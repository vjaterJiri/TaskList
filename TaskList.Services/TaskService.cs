using TaskList.Interfaces.Exceptions;
using TaskList.Interfaces.Model;
using TaskList.Interfaces.Services;
using TaskList.Services.Extensions;
using Task = TaskList.Services.Model.Task;

namespace TaskList.Services
{
	public class TaskService : ITaskService
	{
		private Dictionary<Guid, Task> memoryDb;

		public TaskService()
		{
			this.memoryDb = new Dictionary<Guid, Task>();
		}

		public TaskService(IEnumerable<ITask> defaultValues):this()
		{
			foreach(ITask task in defaultValues)
			{
				this.memoryDb[task.Id] = Task.CreateFromTask(task);
			}
		}

		// Public methods

		public ITask CreateTask(ITask task)
		{
			if (task == null)
			{
				throw new ArgumentNullException(nameof(task));
			}

			Task dbTask = Task.CreateFromTask(task);
			dbTask.Id = Guid.NewGuid();
			this.ExecuteValidations(dbTask);
			this.memoryDb.Add(dbTask.Id, dbTask);
			return Task.CreateFromTask(dbTask);
		}

		public void DeleteTask(Guid id)
		{
			ITask task = this.GetTask(id);
			if(task == null)
			{
				throw new ArgumentException($"Task with id '{id}' does not exist.", nameof(id));
			}

			if(task.Status != ITask.ETaskStatus.Completed)
			{
				throw new ModelValidationException("Only completed task can be deleted.");
			}

			this.memoryDb.Remove(task.Id);

		}

		public IEnumerable<ITask> GetAllTasks()
		{
			return this.memoryDb.Values.Select(x=>Task.CreateFromTask(x));
		}

		public ITask GetTask(Guid id)
		{
			return Task.CreateFromTask(this.GetTaskInternal(id));
		}

		public ITask UpdateTask(ITask task)
		{
			if(task == null)
			{
				throw new ArgumentNullException(nameof(task));	
			}

			Task dbTask = this.GetTaskInternal(task.Id);
			if (dbTask == null)
			{
				throw new ArgumentException($"Task with id '{task.Id}' does not exist.", nameof(task));
			}

			this.ExecuteValidations(Task.CreateFromTask(task));
			dbTask.UpdateFromTask(task);
			return Task.CreateFromTask(dbTask);
		}

		// Private methods

		private Task GetTaskInternal(Guid id)
		{
			return this.memoryDb.SafeGetValue(id, null);
		}

		private void ExecuteValidations(Task task)
		{
			bool isValid;
			string errorMessage;
			(isValid, errorMessage) = this.Validate(task);

			if (!isValid)
			{
				throw new ModelValidationException(errorMessage);
			}
		}

		private (bool status,string message) Validate(Task task)
		{
			if(task.Name == null)
			{
				return (false, $"{nameof(ITask.Name)} is required");
			}

			if(task.Status != ITask.ETaskStatus.InProgress && task.Status != ITask.ETaskStatus.Completed && task.Status != ITask.ETaskStatus.NotStarted)
			{
				return (false, $"Invalid value of {nameof(ITask.Status)} is required");
			}

			if(this.memoryDb.Any(x=>String.Equals(x.Value.Name, task.Name, StringComparison.OrdinalIgnoreCase)&&x.Key != task.Id))
			{
				return (false, $"Task with name '{task.Name}' already exists.");
			}

			return (true, String.Empty);
		}
	}
}
