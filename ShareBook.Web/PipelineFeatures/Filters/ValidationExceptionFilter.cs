using System.Net;
using System.Net.Mime;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShareBook.Application.Common.Exceptions;

namespace ShareBook.Web.PipelineFeatures.Filters;

public class ValidationExceptionFilter : IAsyncExceptionFilter
{
	public Task OnExceptionAsync(ExceptionContext context)
	{
		if (context.Exception is not ModelValidationException exception)
			return Task.CompletedTask;

		context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;
		context.HttpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;

		context.Result = new BadRequestObjectResult(new ErrorResponseModel
		{
			Message = exception.Message,
			StatusCode = (int) HttpStatusCode.BadRequest,
			Errors = exception.Errors
		});
		context.ExceptionHandled = true;
		
		return Task.CompletedTask;
	}

	public class ErrorResponseModel
	{
		public string Message { get; set; }
		public int StatusCode { get; set; }
		public IDictionary<string, string[]> Errors { get; set; }
	}
}