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
		Details = new PersonalDetailsModel
		{
			Id = UserId,
			Name = UserName,
			Claims = accessor.HttpContext.User.Claims.Select(c => new PersonalDetailsModel.Claim()
			{
				Type = c.Type,
				Value = c.Value
			})
		};
	}
	
	public string? UserId { get; }
	public string? UserName { get; }
	public IPersonalDetails? Details { get; }
}