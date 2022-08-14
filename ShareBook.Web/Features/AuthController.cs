using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShareBook.Application.Common.Contracts;
using ShareBook.Application.Identity.Commands;
using ShareBook.Application.Identity.Commands.SignUp;
using ShareBook.Application.Identity.Queries.Authenticate;
using ShareBook.Web.Common;
using ShareBook.Web.Extensions;

namespace ShareBook.Web.Features;
public class AuthController : ApiController
{
	private const string LoginEndpoint = "login";
	private const string MeEndpoint = "me";
	
	[HttpPost]
	[AllowAnonymous]
	[Route(nameof(SignUp))]
	public Task<IActionResult> SignUp(SignUpRequest signUpRequest)
		=> RequestDispatcher
		   .Dispatch<SignUpRequest, AuthenticationResult>(signUpRequest)
		   .ToActionResult();

	[HttpPost]
	[AllowAnonymous]
	[Route(LoginEndpoint)]
	public Task<IActionResult> Authenticate(AuthenticateRequest authRequest)
		=> RequestDispatcher
		   .Dispatch<AuthenticateRequest, AuthenticationResult>(authRequest)
			.ToActionResult();

	[Authorize]
	[HttpGet(MeEndpoint)]
	public IActionResult PersonalDetails([FromServices] ICurrentUser currentUser)
		=> Ok(currentUser.Details);

}