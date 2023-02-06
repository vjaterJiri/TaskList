using TaskList.Interfaces.Exceptions;
using TaskList.Interfaces.Model;
using TaskList.Services;

namespace TaskList.Tests
{
	public partial class TaskServiceTest
	{
		[Test(Description = "Basic scenarion for task delete")]
		public void DeleteTask_ShouldPass()
		{
			TaskImplementation task = new TaskImplementation
			{
				Id = Guid.NewGuid(),
				Name = "test",
				Priority = 7,
				Status = ITask.ETaskStatus.Completed
			};

			TaskService service = new TaskService(new[] { task });


			service.DeleteTask(task.Id);
			Assert.That(service.GetAllTasks().Count(), Is.EqualTo(0));
		}

		[Test(Description = "Deleting not existing task is not allowed")]
		public void DeleteTask_NotExisting_ShouldFail()
		{
			TaskImplementation task = new TaskImplementation
			{
				Id = Guid.NewGuid(),
				Name = "test",
				Priority = 7,
				Status = ITask.ETaskStatus.Completed
			};

			TaskService service = new TaskService(new[] { task });


			Assert.Throws<ArgumentException>(() => service.DeleteTask(Guid.NewGuid()), "Invalid id should fail");
		}

		[Test(Description = "Deleting not completed task is not allowed")]
		public void DeleteTask_NotCompleted_ShouldFail()
		{
			TaskImplementation task = new TaskImplementation
			{
				Id = Guid.NewGuid(),
				Name = "test",
				Priority = 7,
				Status = ITask.ETaskStatus.InProgress
			};

			TaskService service = new TaskService(new[] { task });


			Assert.Throws<ModelValidationException>(() => service.DeleteTask(task.Id), "Only completed tasks can be deleted");
		}
	}
}
