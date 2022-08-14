using Refit;
using ShareBook.ClientSdk.Clients.Posts.Requests;
using ShareBook.ClientSdk.Clients.Posts.Responses;

namespace ShareBook.ClientSdk.Clients.Posts;
public interface IPostClient
{
	[Post("/posts")]
	Task<IApiResponse> Create([AliasAs("image")]StreamPart image, string content);
	
	[Get("/posts/mine")]
	Task<ApiResponse<PostsResponse>> Mine([Authorize] string token);

	[Get("/posts/user/{id:guid}")]
	public Task<ApiResponse<PostsResponse>> ByUser(Guid id, [Authorize] string token);

	[Get("/posts/{id:guid}")]
	public Task<ApiResponse<PostDetailsResponse>> Details(Guid id, [Authorize] string token);

	[Put("/posts/{id:guid}")]
	public Task<IApiResponse> Edit([Body] EditPostRequest request, Guid id, [Authorize] string token);

	[Delete("/posts/{id:guid}")]
	Task<IApiResponse> Delete(Guid id, [Authorize] string token);
}