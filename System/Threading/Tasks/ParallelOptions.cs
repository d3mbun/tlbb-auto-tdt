namespace System.Threading.Tasks
{
	/// <summary>
	/// Stores options that configure the operation of methods on the 
	/// <see cref="T:System.Threading.Tasks.Parallel">Parallel</see> class.
	/// </summary>
	/// <remarks>
	/// By default, methods on the Parallel class attempt to utilize all available processors, are non-cancelable, and target
	/// the default TaskScheduler (TaskScheduler.Default). <see cref="T:System.Threading.Tasks.ParallelOptions" /> enables
	/// overriding these defaults.
	/// </remarks>
	public class ParallelOptions
	{
		private TaskScheduler m_scheduler;

		private int m_maxDegreeOfParallelism;

		private CancellationToken m_cancellationToken;

		/// <summary>
		/// Gets or sets the <see cref="T:System.Threading.Tasks.TaskScheduler">TaskScheduler</see> 
		/// associated with this <see cref="T:System.Threading.Tasks.ParallelOptions" /> instance. Setting this property to null
		/// indicates that the current scheduler should be used.
		/// </summary>
		public TaskScheduler TaskScheduler
		{
			get
			{
				return m_scheduler;
			}
			set
			{
				m_scheduler = value;
			}
		}

		internal TaskScheduler EffectiveTaskScheduler
		{
			get
			{
				if (m_scheduler == null)
				{
					return TaskScheduler.Current;
				}
				return m_scheduler;
			}
		}

		/// <summary>
		/// Gets or sets the maximum degree of parallelism enabled by this ParallelOptions instance.
		/// </summary>
		/// <remarks>
		/// The <see cref="P:System.Threading.Tasks.ParallelOptions.MaxDegreeOfParallelism" /> limits the number of concurrent operations run by <see cref="T:System.Threading.Tasks.Parallel">Parallel</see> method calls that are passed this
		/// ParallelOptions instance to the set value, if it is positive. If <see cref="P:System.Threading.Tasks.ParallelOptions.MaxDegreeOfParallelism" /> is -1, then there is no limit placed on the number of concurrently
		/// running operations.
		/// </remarks>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The exception that is thrown when this <see cref="P:System.Threading.Tasks.ParallelOptions.MaxDegreeOfParallelism" /> is set to 0 or some
		/// value less than -1.
		/// </exception>
		public int MaxDegreeOfParallelism
		{
			get
			{
				return m_maxDegreeOfParallelism;
			}
			set
			{
				if (value == 0 || value < -1)
				{
					throw new ArgumentOutOfRangeException("MaxDegreeOfParallelism");
				}
				m_maxDegreeOfParallelism = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// associated with this <see cref="T:System.Threading.Tasks.ParallelOptions" /> instance.
		/// </summary>
		/// <remarks>
		/// Providing a <see cref="T:System.Threading.CancellationToken">CancellationToken</see>
		/// to a <see cref="T:System.Threading.Tasks.Parallel">Parallel</see> method enables the operation to be
		/// exited early. Code external to the operation may cancel the token, and if the operation observes the
		/// token being set, it may exit early by throwing an
		/// <see cref="T:System.OperationCanceledException" />.
		/// </remarks>
		public CancellationToken CancellationToken
		{
			get
			{
				return m_cancellationToken;
			}
			set
			{
				m_cancellationToken = value;
			}
		}

		internal int EffectiveMaxConcurrencyLevel
		{
			get
			{
				int num = MaxDegreeOfParallelism;
				int maximumConcurrencyLevel = EffectiveTaskScheduler.MaximumConcurrencyLevel;
				if (maximumConcurrencyLevel > 0 && maximumConcurrencyLevel != int.MaxValue)
				{
					num = ((num == -1) ? maximumConcurrencyLevel : Math.Min(maximumConcurrencyLevel, num));
				}
				return num;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Threading.Tasks.ParallelOptions" /> class.
		/// </summary>
		/// <remarks>
		/// This constructor initializes the instance with default values.  <see cref="P:System.Threading.Tasks.ParallelOptions.MaxDegreeOfParallelism" />
		/// is initialized to -1, signifying that there is no upper bound set on how much parallelism should
		/// be employed.  <see cref="P:System.Threading.Tasks.ParallelOptions.CancellationToken" /> is initialized to a non-cancelable token,
		/// and <see cref="P:System.Threading.Tasks.ParallelOptions.TaskScheduler" /> is initialized to the default scheduler (TaskScheduler.Default).  
		/// All of these defaults may be overwritten using the property set accessors on the instance.
		/// </remarks>
		public ParallelOptions()
		{
			m_scheduler = TaskScheduler.Default;
			m_maxDegreeOfParallelism = -1;
			m_cancellationToken = CancellationToken.None;
		}
	}
}
