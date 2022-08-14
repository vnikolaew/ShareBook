using Refit;
using ShareBook.ClientSdk.Clients.Comments.Requests;

namespace ShareBook.ClientSdk.Clients.Comments;
public interface ICommentsClient
{
	[Post("/comments/{id:guid}")]
	Task<IApiResponse> Comment([Body] CreateCommentRequest request, Guid id, [Authorize] string token);

	[Put("/comments/{id:guid}")]
	public Task<IApiResponse> Edit([Body] EditCommentRequest request, Guid id, [Authorize] string token);
}