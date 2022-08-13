using Microsoft.AspNetCore.Mvc;
using ShareBook.Application.Common;
using ShareBook.Shared.Abstractions.Requests;
using ShareBook.Web.Extensions;

namespace ShareBook.Web.Common;

[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{
	protected const char PathSeparator = '/';
	protected const string GuidId = "{id:guid}";

	public IRequestDispatcher RequestDispatcher
		=> HttpContext
		   .RequestServices
		   .GetRequiredService<IRequestDispatcher>();

	protected Task<IActionResult> SendAsync<TRequest>(TRequest request)
		where TRequest : IRequest<Result>
		=> RequestDispatcher
		   .Dispatch<TRequest, Result>(request)
		   .ToActionResult();

	protected Task<IActionResult> SendAsync<TRequest, TResult>(TRequest request)
		where TRequest : IRequest<Result<TResult>>
		=> RequestDispatcher
		   .Dispatch<TRequest, Result<TResult>>(request)
		   .ToActionResult();	
}