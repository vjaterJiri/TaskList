using System;
using System.Collections.Generic;
using TaskList.Interfaces.Model;

namespace TaskList.Interfaces.Services
{
	public interface ITaskService
	{
		public ITask GetTask(Guid id);

		public IEnumerable<ITask> GetAllTasks();

		public ITask UpdateTask(ITask task);

		public ITask CreateTask(ITask task);

		public void DeleteTask(Guid task);
	}
}
