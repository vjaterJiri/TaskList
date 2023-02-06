using System.ComponentModel.DataAnnotations;
using TaskList.Interfaces.Model;
using TaskList.Services;

namespace TaskList.Tests
{
	public partial class TaskServiceTest
	{
		private class TaskImplementation : ITask
		{
			public Guid Id { get; set; }
			public string Name { get; set; }
			public int Priority { get; set; }
			public ITask.ETaskStatus Status { get; set; }
		}

		[SetUp]
		public void Setup()
		{
		}		
	}
}