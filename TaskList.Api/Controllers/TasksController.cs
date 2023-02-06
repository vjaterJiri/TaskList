using Microsoft.AspNetCore.Mvc;
using TaskList.Interfaces.Services;
using TaskList.Api.Model;
using TaskList.Interfaces.Model;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using TaskList.Api.Model.Api;
using TaskList.Interfaces.Exceptions;

namespace TaskList.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TasksController : ControllerBase
	{
		private readonly ILogger<TasksController> logger;
		private readonly ITaskService service;

		public TasksController(ILogger<TasksController> logger, ITaskService service)
		{
			this.logger = logger;
			this.service = service;
		}

		[HttpGet]
		[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ApiTask))]
		public IActionResult Get()
		{
			return this.Ok(this.service.GetAllTasks().Select(x => ApiTask.CreateFromInterface(x)));
		}

		[HttpGet]
		[Route("{id}")]
		[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ApiTask))]
		[SwaggerResponse((int)HttpStatusCode.NotFound)]
		public IActionResult Get(Guid id)
		{
			ITask task = this.service.GetTask(id);
			if (task == null)
			{
				return this.NotFound();
			}
			return this.Ok(ApiTask.CreateFromInterface(task));
		}

		[HttpPost]
		[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ApiTask))]
		[SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
		public IActionResult Create([FromBody]ApiTaskInput task)
		{
			try
			{
				return this.Ok(ApiTask.CreateFromInterface(this.service.CreateTask(ServiceTask.CreateFromApi(task))));
			}
			catch (ModelValidationException ex)
			{
				return this.ValidationProblem(new ValidationProblemDetails
				{
					Detail = ex.Message
				});
			}
		}

		[HttpPut]
		[Route("{id}")]
		[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ApiTask))]
		[SwaggerResponse((int)HttpStatusCode.NotFound)]
		[SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
		public IActionResult Update([FromBody] ApiTaskInput task, Guid id)
		{
			try
			{
				ServiceTask serviceTask = ServiceTask.CreateFromApi(task);
				serviceTask.Id = id;
				return this.Ok(ApiTask.CreateFromInterface(this.service.UpdateTask(serviceTask)));
			}
			catch (ModelValidationException ex)
			{
				return this.ValidationProblem(new ValidationProblemDetails
				{
					Detail = ex.Message
				});
			}
		}

		[HttpDelete]
		[Route("{id}")]
		[SwaggerResponse((int)HttpStatusCode.NoContent)]
		[SwaggerResponse((int)HttpStatusCode.NotFound)]
		[SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(ValidationProblemDetails))]
		public IActionResult Delete(Guid id)
		{
			try
			{
				this.service.DeleteTask(id);
				return this.NoContent();
			}
			catch (Exception ex)
			{
				return this.ValidationProblem(new ValidationProblemDetails
				{
					Detail = ex.Message
				});
			}
		}
	}
}
