﻿namespace TaskList.Interfaces.Exceptions
{
	public class ModelValidationException : Exception
	{
		public ModelValidationException(string message):base(message){ }
	}
}
