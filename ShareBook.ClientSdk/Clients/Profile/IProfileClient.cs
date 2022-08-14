using Refit;
using ShareBook.ClientSdk.Clients.Profile.Requests;
using ShareBook.ClientSdk.Clients.Profile.Responses;

namespace ShareBook.ClientSdk.Clients.Profile;
public interface IProfileClient
{
	[Put("/profile")]
	Task<IApiResponse> Edit(EditProfileRequest request, [Authorize] string token);

	[Put("/profile/photo")]
	Task<IApiResponse> EditProfilePhoto([AliasAs("image")]StreamPart image, [Authorize] string token);

	[Get("/profile")]
	Task<ApiResponse<ProfileResponse>> Mine( [Authorize] string token);
	
	[Get("/profile/{id:guid}")]
	Task<ApiResponse<ProfileResponse>> ByUser(Guid id, [Authorize] string token);
}