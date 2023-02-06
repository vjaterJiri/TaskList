using System;

namespace TaskList.Interfaces.Model
{
	public interface ITask
	{
		public enum ETaskStatus 
		{
			NotStarted = 0,			
			InProgress = 1,
			Completed = 2
		}

		Guid Id { get; set; }

		string Name { get; set; }

		int Priority { get; set; }

		ETaskStatus Status { get; set; }
	}
}
