using System;
using TaskList.Interfaces.Model;

namespace TaskList.Api.Model.Api
{
	public class ApiTask : ApiTaskInput
	{
		public Guid Id { get; set; }

		
		public static ApiTask CreateFromInterface(ITask source)
		{
			ApiTask result = new ApiTask();
			result.Id = source.Id;
			result.Name = source.Name;
			result.Priority = source.Priority;
			result.Status = (ETaskStatus)source.Status;

			return result;
		}
	}
}
