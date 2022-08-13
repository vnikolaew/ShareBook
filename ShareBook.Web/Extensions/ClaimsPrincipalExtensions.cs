using System.Security.Claims;

namespace ShareBook.Web.Extensions;

public static class ClaimsPrincipalExtensions
{
	public static string? GetId(this ClaimsPrincipal claimsPrincipal)
		=> claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

	public static string? GetEmail(this ClaimsPrincipal claimsPrincipal)
		=> claimsPrincipal.FindFirstValue(ClaimTypes.Email);

	public static string? GetUserName(this ClaimsPrincipal claimsPrincipal)
		=> claimsPrincipal.FindFirstValue(ClaimTypes.Name);	
}