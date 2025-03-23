namespace System.Threading.Tasks
{
	internal class ParallelForReplicaTask : Task
	{
		internal object m_stateForNextReplica;

		internal object m_stateFromPreviousReplica;

		internal Task m_handedOverChildReplica;

		internal override object SavedStateForNextReplica
		{
			get
			{
				return m_stateForNextReplica;
			}
			set
			{
				m_stateForNextReplica = value;
			}
		}

		internal override object SavedStateFromPreviousReplica
		{
			get
			{
				return m_stateFromPreviousReplica;
			}
			set
			{
				m_stateFromPreviousReplica = value;
			}
		}

		internal override Task HandedOverChildReplica
		{
			get
			{
				return m_handedOverChildReplica;
			}
			set
			{
				m_handedOverChildReplica = value;
			}
		}

		internal ParallelForReplicaTask(Action<object> taskReplicaDelegate, object stateObject, Task parentTask, TaskScheduler taskScheduler, TaskCreationOptions creationOptionsForReplica, InternalTaskOptions internalOptionsForReplica)
			: base(taskReplicaDelegate, stateObject, parentTask, CancellationToken.None, creationOptionsForReplica, internalOptionsForReplica, taskScheduler)
		{
		}

		/// <summary>
		/// In some cases a replica will want to quit prematurely (ie. before finishing a chunk of work it may have grabbed)
		/// yet they will need the next replica to pick things up from where they left. This API is used to save such state.
		///
		/// Calling it is also the only way to record a premature exit.
		/// </summary>
		/// <param name="stateForNextReplica"></param>
		internal void SaveStateForNextReplica(object stateForNextReplica)
		{
			m_stateForNextReplica = stateForNextReplica;
		}
	}
}
