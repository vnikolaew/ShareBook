using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
	public IActionResult PersonalDetails()
		=> Ok(new PersonalDetailsModel
		{
			Name = User.Identity.Name,
			Id = User.GetId(),
			Claims = User.Claims.Select(c => new PersonalDetailsModel.Claim()
			{
				Type = c.Type,
				Value = c.Value
			})
		});

	private class PersonalDetailsModel
	{
		public string Name { get; set; }
		public string Id { get; set; }
		public IEnumerable<Claim> Claims { get; set; }

		public class Claim
		{
			public string Type { get; set; }
			public string Value { get; set; }
		}
	}	
}