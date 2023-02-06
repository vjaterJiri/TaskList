using System;
using TaskList.Interfaces.Model;

namespace TaskList.Services.Model
{
	internal class Task : ITask
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int Priority { get; set; }
		public ITask.ETaskStatus Status { get; set; }

		public static Task CreateFromTask(ITask source)
		{
			if(source == null)
			{
				return null;
			}

			Task result = new Task();
			result.Id = source.Id;
			result.Name = source.Name;
			result.Priority = source.Priority;
			result.Status = source.Status;

			return result;
		}


		public void UpdateFromTask(ITask source)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}

			this.Id = source.Id;
			this.Name = source.Name;
			this.Priority = source.Priority;
			this.Status = source.Status;
		}
	}
}
