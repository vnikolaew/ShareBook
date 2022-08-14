using Refit;
using ShareBook.ClientSdk.Clients.Follows.Responses;

namespace ShareBook.ClientSdk.Clients.Follows;

public interface IFollowsClient
{
	[Post("/follows/{id:guid}")]
	Task<IApiResponse> Follow(Guid id, [Authorize] string token);
	
	
	[Delete("/follows/{id:guid}")]
	Task<IApiResponse> Unfollow(Guid id, [Authorize] string token);

	[Get("/followers")]
	Task<ApiResponse<FollowersResponse>> Followers([Authorize] string token);
	
	[Get("/following")]
	Task<ApiResponse<FollowersResponse>> Following([Authorize]string token);

}