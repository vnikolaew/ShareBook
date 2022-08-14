using System.Security.Claims;
using ShareBook.Application.Common.Contracts;
using ShareBook.Web.Extensions;

namespace ShareBook.Web.Services;

public class CurrentUserService : ICurrentUser
{
	public CurrentUserService(IHttpContextAccessor accessor)
	{
		var claimsPrincipal
			= accessor.HttpContext.User
			  ?? throw new InvalidOperationException("The request does not have an authenticated user.");

		UserId = claimsPrincipal.GetId();
		UserName = claimsPrincipal.GetUserName();
	}
	
	public string? UserId { get; }
	public string? UserName { get; }
}