using System;
using TaskList.Api.Model.Api;
using TaskList.Interfaces.Model;

namespace TaskList.Api.Model
{
	public class ServiceTask: ITask
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public int Priority { get; set; }
		
		public ITask.ETaskStatus Status { get; set; }


		public static ServiceTask CreateFromInterface(ITask source)
		{
			ServiceTask result = new ServiceTask();
			result.Id = source.Id;
			result.Name = source.Name;
			result.Priority = source.Priority;
			result.Status = source.Status;

			return result;
		}

		public static ServiceTask CreateFromApi(ApiTaskInput source)
		{
			ServiceTask result = new ServiceTask();
			result.Name = source.Name;
			result.Priority = source.Priority;
			result.Status = (ITask.ETaskStatus)source.Status;

			return result;
		}
	}
}
