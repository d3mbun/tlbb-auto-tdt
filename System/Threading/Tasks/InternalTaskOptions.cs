namespace System.Threading.Tasks
{
	/// <summary>
	/// Task creation flags which are only used internally.
	/// </summary>
	[Serializable]
	[Flags]
	internal enum InternalTaskOptions
	{
		/// <summary> Specifies "No internal task options" </summary>
		None = 0x0,
		/// <summary>Used to filter out internal vs. public task creation options.</summary>
		InternalOptionsMask = 0xFF00,
		ChildReplica = 0x100,
		ContinuationTask = 0x200,
		PromiseTask = 0x400,
		SelfReplicating = 0x800,
		/// <summary>Specifies that the task will be queued by the runtime before handing it over to the user. 
		/// This flag will be used to skip the cancellationtoken registration step, which is only meant for unstarted tasks.</summary>
		QueuedByRuntime = 0x2000
	}
}
