using ShareBook.Application.Identity.Commands;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Identity.Queries.Authenticate;
public class AuthenticateRequest : IRequest<AuthenticationResult>
{
	public string Email { get; set; }
	public string Password { get; set; }

	public class AuthenticateHandler : IRequestHandler<AuthenticateRequest, AuthenticationResult>
	{
		private readonly IIdentityService _identityService;

		public AuthenticateHandler(IIdentityService identityService)
			=> _identityService = identityService;

		public async Task<AuthenticationResult> Handle(
			AuthenticateRequest query,
			CancellationToken cancellationToken)
		{
			var authRequestModel = new AuthRequestModel
			{
				Email = query.Email,
				Password = query.Password
			};

			return await _identityService.Authenticate(authRequestModel);
		}
	}
}