using TaskList.Interfaces.Exceptions;
using TaskList.Interfaces.Model;
using TaskList.Services;

namespace TaskList.Tests
{
	public partial class TaskServiceTest
	{
		[Test(Description = "Basic scenarion for task creation")]
		public void CreateTask_ShouldPass()
		{
			TaskService service = new TaskService();
			TaskImplementation task = new TaskImplementation
			{
				Name = "test",
				Priority = 7,
				Status = ITask.ETaskStatus.NotStarted
			};

			ITask result = service.CreateTask(task);
			Assert.IsNotNull(result);
			Assert.That(result.Name, Is.EqualTo(task.Name));
			Assert.That(result.Priority, Is.EqualTo(task.Priority));
			Assert.That(result.Status, Is.EqualTo(task.Status));
			Assert.That(Guid.Empty, Is.Not.EqualTo(result.Id));
		}

		[Test(Description = "Creating tasks with different name passes")]
		public void CreateTask_AnotherTask_ShouldPass()
		{
			TaskService service = new TaskService();
			TaskImplementation task = new TaskImplementation
			{
				Name = "test",
				Priority = 7,
				Status = ITask.ETaskStatus.NotStarted
			};

			service.CreateTask(task);
			task.Name = "test 2";
			task.Priority = 2;
			task.Status = ITask.ETaskStatus.NotStarted;

			ITask result = service.CreateTask(task);
			Assert.IsNotNull(result);
			Assert.That(result.Name, Is.EqualTo(task.Name));
			Assert.That(result.Priority, Is.EqualTo(task.Priority));
			Assert.That(result.Status, Is.EqualTo(task.Status));
			Assert.That(Guid.Empty, Is.Not.EqualTo(result.Id));


		}

		[Test(Description = "Creating tasks with same name is not allowed")]
		public void CreateTask_Duplicate_ShouldFail()
		{
			TaskService service = new TaskService();
			TaskImplementation task = new TaskImplementation
			{
				Name = "test",
				Priority = 7,
				Status = ITask.ETaskStatus.NotStarted
			};

			ITask result = service.CreateTask(task);
			task.Priority = 2;
			task.Status = ITask.ETaskStatus.InProgress;
			Assert.Throws<ModelValidationException>(() => service.CreateTask(task), "Duplicate task name should cause exception");

		}

		[Test(Description = "Creating tasks with empty name is not allowed")]
		public void CreateTask_EmptyName_ShouldFail()
		{
			TaskService service = new TaskService();
			TaskImplementation task = new TaskImplementation
			{
				Priority = 7,
				Status = ITask.ETaskStatus.NotStarted
			};

			task.Priority = 2;
			task.Status = ITask.ETaskStatus.InProgress;
			Assert.Throws<ModelValidationException>(() => service.CreateTask(task), "Empty task name should cause exception");

		}

		[Test(Description = "Creating tasks with null is not allowed")]
		public void CreateTask_Null_ShouldFail()
		{
			TaskService service = new TaskService();
			Assert.Throws<ArgumentNullException>(() => service.CreateTask(null), "Can not create with null object");
		}
	}
}
