using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TaskList.Api.Model.Api
{
	public class ApiTaskInput
	{
		public enum ETaskStatus
		{
			[EnumMember(Value = "Not Started")]
			NotStarted = 0,
			[EnumMember(Value = "In Progress")]
			InProgress = 1,
			Completed = 2
		}

		[Required]
		public string Name { get; set; }

		[Range(1,Int32.MaxValue)]
		public int Priority { get; set; }

		public ETaskStatus Status { get; set; }


	}
}
