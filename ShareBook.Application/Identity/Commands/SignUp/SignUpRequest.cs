using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Identity.Commands.SignUp;
public class SignUpRequest : IRequest<AuthenticationResult>
{
	public string Username { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }

	public class SignUpRequestHandler : IRequestHandler<SignUpRequest, AuthenticationResult>
	{
		private readonly IIdentityService _identityService;

		public SignUpRequestHandler(IIdentityService identityService)
			=> _identityService = identityService;

		public async Task<AuthenticationResult> Handle(
			SignUpRequest request,
			CancellationToken cancellationToken)
		{
			var requestModel = new SignUpRequestModel
			{
				Username = request.Username,
				Email = request.Email,
				Password = request.Password
			};
			
			return await _identityService.SignUpAsync(requestModel);
		}
	}
}