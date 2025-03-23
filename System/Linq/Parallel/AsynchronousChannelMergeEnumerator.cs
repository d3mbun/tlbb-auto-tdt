using System.Threading;

namespace System.Linq.Parallel
{
	/// <summary>
	/// An enumerator that merges multiple one-to-one channels into a single output
	/// stream, including any necessary blocking and synchronization. This is an
	/// asynchronous enumerator, i.e. the producers may be inserting items into the
	/// channels concurrently with the consumer taking items out of them. Therefore,
	/// enumerating this object can cause the current thread to block.
	///
	/// We use a biased choice algorithm to choose from our consumer channels. I.e. we
	/// will prefer to process elements in a fair round-robin fashion, but will
	/// occassionally bypass this if a channel is empty.
	///
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal sealed class AsynchronousChannelMergeEnumerator<T> : MergeEnumerator<T>
	{
		private AsynchronousChannel<T>[] m_channels;

		private ManualResetEventSlim[] m_channelEvents;

		private bool[] m_done;

		private int m_channelIndex;

		private T m_currentElement;

		public override T Current
		{
			get
			{
				if (m_channelIndex == -1 || m_channelIndex == m_channels.Length)
				{
					throw new InvalidOperationException("PLINQ_CommonEnumerator_Current_NotStarted");
				}
				return m_currentElement;
			}
		}

		internal AsynchronousChannelMergeEnumerator(QueryTaskGroupState taskGroupState, AsynchronousChannel<T>[] channels)
			: base(taskGroupState)
		{
			m_channels = channels;
			m_channelIndex = -1;
			m_done = new bool[m_channels.Length];
		}

		public override bool MoveNext()
		{
			int num = m_channelIndex;
			if (num == -1)
			{
				num = (m_channelIndex = 0);
			}
			if (num == m_channels.Length)
			{
				return false;
			}
			if (!m_done[num] && m_channels[num].TryDequeue(ref m_currentElement))
			{
				m_channelIndex = (num + 1) % m_channels.Length;
				return true;
			}
			return MoveNextSlowPath();
		}

		private bool MoveNextSlowPath()
		{
			int num = 0;
			int num2 = m_channelIndex;
			int channelIndex;
			while ((channelIndex = m_channelIndex) != m_channels.Length)
			{
				AsynchronousChannel<T> asynchronousChannel = m_channels[channelIndex];
				bool flag = m_done[channelIndex];
				if (!flag && asynchronousChannel.TryDequeue(ref m_currentElement))
				{
					m_channelIndex = (channelIndex + 1) % m_channels.Length;
					return true;
				}
				if (!flag && asynchronousChannel.IsDone)
				{
					if (!asynchronousChannel.IsChunkBufferEmpty)
					{
						asynchronousChannel.TryDequeue(ref m_currentElement);
						return true;
					}
					m_done[channelIndex] = true;
					if (m_channelEvents != null)
					{
						m_channelEvents[channelIndex] = null;
					}
					flag = true;
					asynchronousChannel.Dispose();
				}
				if (flag && ++num == m_channels.Length)
				{
					channelIndex = (m_channelIndex = m_channels.Length);
					break;
				}
				channelIndex = (m_channelIndex = (channelIndex + 1) % m_channels.Length);
				if (channelIndex != num2)
				{
					continue;
				}
				try
				{
					if (m_channelEvents == null)
					{
						m_channelEvents = new ManualResetEventSlim[m_channels.Length];
					}
					num = 0;
					for (int i = 0; i < m_channels.Length; i++)
					{
						if (!m_done[i] && m_channels[i].TryDequeue(ref m_currentElement, ref m_channelEvents[i]))
						{
							return true;
						}
						if (m_channelEvents[i] == null)
						{
							if (!m_done[i])
							{
								m_done[i] = true;
								m_channels[i].Dispose();
							}
							if (++num == m_channels.Length)
							{
								channelIndex = (m_channelIndex = m_channels.Length);
								break;
							}
						}
					}
					if (channelIndex == m_channels.Length)
					{
						break;
					}
					num2 = (m_channelIndex = WaitAny(m_channelEvents));
					num = 0;
					continue;
				}
				finally
				{
					for (int j = 0; j < m_channelEvents.Length; j++)
					{
						if (m_channelEvents[j] != null)
						{
							m_channels[j].DoneWithDequeueWait();
						}
					}
				}
			}
			m_taskGroupState.QueryEnd(userInitiatedDispose: false);
			return false;
		}

		/// <summary>
		/// WaitAny simulates a Win32-style WaitAny on the set of thin-events.
		/// </summary>
		/// <param name="events">An array of thin-events (null elements permitted)</param>
		/// <returns>The index of the specific event in events that caused us to wake up.</returns>
		private static int WaitAny(ManualResetEventSlim[] events)
		{
			SpinWait spinWait = default(SpinWait);
			for (int i = 0; i < 20; i++)
			{
				for (int j = 0; j < events.Length; j++)
				{
					if (events[j] != null && events[j].IsSet)
					{
						return j;
					}
				}
				spinWait.SpinOnce();
			}
			int num = 0;
			for (int k = 0; k < events.Length; k++)
			{
				if (events[k] == null)
				{
					num++;
				}
			}
			WaitHandle[] array = new WaitHandle[events.Length - num];
			int l = 0;
			int num2 = 0;
			for (; l < events.Length; l++)
			{
				if (events[l] != null)
				{
					array[num2] = events[l].WaitHandle;
					num2++;
				}
			}
			int num3 = WaitHandle.WaitAny(array);
			int m = 0;
			int num4 = -1;
			for (; m < events.Length; m++)
			{
				if (events[m] != null)
				{
					num4++;
					if (num4 == num3)
					{
						num3 = m;
						break;
					}
				}
			}
			return num3;
		}
	}
}
