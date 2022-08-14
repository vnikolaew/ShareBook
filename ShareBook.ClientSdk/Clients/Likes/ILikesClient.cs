using Refit;
using ShareBook.ClientSdk.Clients.Likes.Responses;

namespace ShareBook.ClientSdk.Clients.Likes;

public interface ILikesClient
{
	[Post("/likes/{id:guid}")]
	Task<IApiResponse> Like([AliasAs("id")] Guid postId);
	
	[Delete("/likes/{id:guid}")]
	Task<IApiResponse> Unlike([AliasAs("id")] Guid postId);

	[Get("/likes/{id:guid}")]
	Task<ApiResponse<LikesResponse>> ByPost(Guid id);
}