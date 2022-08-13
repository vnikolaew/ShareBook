using System.Net;
using Microsoft.AspNetCore.Mvc;
using ShareBook.Application.Common;
using ShareBook.Application.Identity.Commands;

namespace ShareBook.Web.Extensions;

public static class ResultExtensions
{
	public static IActionResult ToActionResult(this Result result)
		=> result.Succeeded
			? new OkResult()
			: new BadRequestObjectResult(result.Errors);

	public static IActionResult ToActionResult<TResult>(this Result<TResult> result)
	 => result.Succeeded
		 ? new OkObjectResult(result.Data)
		 : new BadRequestObjectResult(result.Errors);

	public static async Task<IActionResult> ToActionResult(this Task<Result> task)
	{
		var result = await task;
		return result.Succeeded
			? new OkResult()
			: new BadRequestObjectResult(result.Errors);
	}

	public static async Task<IActionResult> ToActionResult<TResult>(
		this Task<Result<TResult>> task)
	{
		var result = await task;
		return result.Succeeded
			? new OkObjectResult(result.Data)
			: new BadRequestObjectResult(result.Errors);
	}

	public static async Task<IActionResult> OkOrNotFound<TResult>(
		this Task<Result<TResult>> task)
	{
		var result = await task;
		return result.Succeeded
			? new OkObjectResult(result.Data)
			: new NotFoundObjectResult(result.Errors);
	}

	public static async Task<IActionResult> AcceptedOrBadRequest(
		this Task<Result> task)
	{
		var result = await task;
		return result.Succeeded
			? new AcceptedResult()
			 : new BadRequestObjectResult(result.Errors);
	}

	public static IActionResult ToActionResult(this AuthenticationResult result)
		=> result.Succeeded
			? new OkObjectResult(new
			{
				Id = result.UserId, result.Token
			})
			: new BadRequestObjectResult(new
			{
				Status = HttpStatusCode.BadRequest, result.Errors
			});
	
	public static async Task<IActionResult> ToActionResult(this Task<AuthenticationResult> task)
	{
		var result = await task;
		return result.Succeeded
			? new OkObjectResult(new
			{
				Id = result.UserId, result.Token
			})
			: new BadRequestObjectResult(new
			{
				Status = HttpStatusCode.BadRequest, result.Errors
			});
	}
}