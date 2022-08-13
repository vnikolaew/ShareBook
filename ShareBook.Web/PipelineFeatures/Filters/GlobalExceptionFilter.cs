using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShareBook.Web.PipelineFeatures.Filters;
public class GlobalExceptionFilter : IAsyncExceptionFilter
{
	public Task OnExceptionAsync(ExceptionContext context)
	{
		context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;
		context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

		var exception = context.Exception;
		context.Result = new BadRequestObjectResult(new 
		{
			exception.Message,
			StatusCode = (int) HttpStatusCode.BadRequest,
		});
		
		context.ExceptionHandled = true;
		return Task.CompletedTask;
	}
}