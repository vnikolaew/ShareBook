using Refit;
using ShareBook.ClientSdk.Clients.Requests;
using ShareBook.ClientSdk.Clients.Responses;

namespace ShareBook.ClientSdk.Clients;
public interface IAuthClient
{
	[Post("/auth/login")]
	Task<ApiResponse<AuthenticationResult>> Authenticate([Body] AuthenticateRequest request);
	
	[Post("/auth/signup")]
	Task<ApiResponse<AuthenticationResult>> SignUp([Body] SignUpRequest request);

	[Get("/auth/me")]
	Task<ApiResponse<PersonalDetails>> PersonalDetails([Authorize] string token);
}