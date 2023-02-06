using TaskList.Interfaces.Exceptions;
using TaskList.Interfaces.Model;
using TaskList.Services;

namespace TaskList.Tests
{
	public partial class TaskServiceTest
	{
		[Test(Description = "Basic scenarion for task update")]
		public void UpdateTask_ShouldPass()
		{
			TaskImplementation task = new TaskImplementation
			{
				Id = Guid.NewGuid(),
				Name = "test",
				Priority = 7,
				Status = ITask.ETaskStatus.NotStarted
			};

			TaskService service = new TaskService(new[] { task });

			TaskImplementation updateTask = new TaskImplementation
			{
				Id = task.Id,
				Name = "test XXXXX",
				Priority = 2,
				Status = ITask.ETaskStatus.InProgress
			};

			ITask result = service.UpdateTask(updateTask);
			Assert.IsNotNull(result);
			Assert.That(result.Name, Is.EqualTo(updateTask.Name));
			Assert.That(result.Priority, Is.EqualTo(updateTask.Priority));
			Assert.That(result.Status, Is.EqualTo(updateTask.Status));
			Assert.That(result.Id, Is.EqualTo(updateTask.Id));
		}

		[Test(Description = "Updating tasks with empty name is not allowed")]
		public void UpdateTask_EmptyName_SouldFail()
		{
			TaskImplementation task = new TaskImplementation
			{
				Id = Guid.NewGuid(),
				Name = "test",
				Priority = 7,
				Status = ITask.ETaskStatus.NotStarted
			};

			TaskService service = new TaskService(new[] { task });

			TaskImplementation updateTask = new TaskImplementation
			{
				Id = task.Id,
				Priority = 2,
				Status = ITask.ETaskStatus.InProgress
			};

			Assert.Throws<ModelValidationException>(() => service.UpdateTask(updateTask), "Empty task name should cause exception");

		}

		[Test(Description = "Updating tasks with same name is not allowed")]
		public void UpdateTask_Duplicate_ShouldFail()
		{
			TaskImplementation task = new TaskImplementation
			{
				Id = Guid.NewGuid(),
				Name = "test",
				Priority = 7,
				Status = ITask.ETaskStatus.NotStarted
			};

			TaskImplementation task2 = new TaskImplementation
			{
				Id = Guid.NewGuid(),
				Name = "test 1",
				Priority = 7,
				Status = ITask.ETaskStatus.NotStarted
			};

			TaskService service = new TaskService(new[] { task, task2 });

			TaskImplementation updateTask = new TaskImplementation
			{
				Id = task.Id,
				Name = task2.Name,
				Priority = 2,
				Status = ITask.ETaskStatus.InProgress
			};

			Assert.Throws<ModelValidationException>(() => service.UpdateTask(updateTask), "Duplicate task name should cause exception");

		}

		[Test(Description = "Updating tasks with null is not allowed")]
		public void UpdateTask_Null_ShouldFail()
		{
			TaskImplementation task = new TaskImplementation
			{
				Id = Guid.NewGuid(),
				Name = "test",
				Priority = 7,
				Status = ITask.ETaskStatus.NotStarted
			};

			TaskImplementation task2 = new TaskImplementation
			{
				Id = Guid.NewGuid(),
				Name = "test 1",
				Priority = 7,
				Status = ITask.ETaskStatus.NotStarted
			};

			TaskService service = new TaskService(new[] { task, task2 });
			Assert.Throws<ArgumentNullException>(() => service.UpdateTask(null), "Can not update with null object");
		}

		[Test(Description = "Updating not exitsting is not allowed")]
		public void UpdateTask_NotExisting_ShouldFail()
		{
			TaskImplementation task = new TaskImplementation
			{
				Id = Guid.NewGuid(),
				Name = "test",
				Priority = 7,
				Status = ITask.ETaskStatus.NotStarted
			};

			TaskImplementation task2 = new TaskImplementation
			{
				Id = Guid.NewGuid(),
				Name = "test 1",
				Priority = 7,
				Status = ITask.ETaskStatus.NotStarted
			};

			TaskImplementation updateTask = new TaskImplementation
			{
				Id = Guid.NewGuid(),
				Name = "test X",
				Priority = 7,
				Status = ITask.ETaskStatus.NotStarted
			};

			TaskService service = new TaskService(new[] { task, task2 });
			Assert.Throws<ArgumentException>(() => service.UpdateTask(updateTask), "Can not update not existing task");
		}
	}
}
