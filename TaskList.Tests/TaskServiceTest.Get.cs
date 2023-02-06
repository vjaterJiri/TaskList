using TaskList.Interfaces.Model;
using TaskList.Services;

namespace TaskList.Tests
{
	public partial class TaskServiceTest
	{
		[Test(Description = "Basic scenario for get all tasks")]
		public void GetAllTasks_ShouldPass()
		{
			TaskImplementation task = new TaskImplementation
			{
				Id = Guid.NewGuid(),
				Name = "test",
				Priority = 7,
				Status = ITask.ETaskStatus.InProgress
			};

			TaskImplementation task2 = new TaskImplementation
			{
				Id = Guid.NewGuid(),
				Name = "test 2",
				Priority = 7,
				Status = ITask.ETaskStatus.InProgress
			};

			ITask[] tasks = new[] { task, task2 };

			TaskService service = new TaskService(tasks);

			ITask[] resultTasks = service.GetAllTasks().ToArray();
			Assert.That(resultTasks.Length, Is.EqualTo(tasks.Length));
			foreach (ITask resultTask in tasks)
			{
				ITask expected = tasks.FirstOrDefault(x => x.Id == resultTask.Id);
				Assert.IsNotNull(expected, "Inconsistant ids");
				Assert.That(resultTask.Name, Is.EqualTo(expected.Name));
				Assert.That(resultTask.Priority, Is.EqualTo(expected.Priority));
				Assert.That(resultTask.Status, Is.EqualTo(expected.Status));
			}

		}

		[Test(Description = "Basic scenario for get task")]
		public void GetTasks_ShouldPass()
		{
			TaskImplementation task = new TaskImplementation
			{
				Id = Guid.NewGuid(),
				Name = "test",
				Priority = 7,
				Status = ITask.ETaskStatus.InProgress
			};

			TaskImplementation task2 = new TaskImplementation
			{
				Id = Guid.NewGuid(),
				Name = "test 2",
				Priority = 7,
				Status = ITask.ETaskStatus.InProgress
			};

			ITask[] tasks = new[] { task, task2 };

			TaskService service = new TaskService(tasks);

			ITask result = service.GetTask(task.Id);
			Assert.IsNotNull(result, "Task should be found by its ID");
			Assert.That(result.Name, Is.EqualTo(task.Name));
			Assert.That(result.Priority, Is.EqualTo(task.Priority));
			Assert.That(result.Status, Is.EqualTo(task.Status));
			Assert.That(result.Id, Is.EqualTo(task.Id));
		}

		[Test(Description = "return null if task not found")]
		public void GetTasks_InvalidId_IsNull()
		{
			TaskImplementation task = new TaskImplementation
			{
				Id = Guid.NewGuid(),
				Name = "test",
				Priority = 7,
				Status = ITask.ETaskStatus.InProgress
			};

			TaskImplementation task2 = new TaskImplementation
			{
				Id = Guid.NewGuid(),
				Name = "test 2",
				Priority = 7,
				Status = ITask.ETaskStatus.InProgress
			};

			ITask[] tasks = new[] { task, task2 };

			TaskService service = new TaskService(tasks);

			ITask result = service.GetTask(Guid.NewGuid());
			Assert.IsNull(result, "Invalid id should return null");
		}
	}
}
