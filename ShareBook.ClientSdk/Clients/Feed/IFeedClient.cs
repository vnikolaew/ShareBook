using Refit;
using ShareBook.ClientSdk.Clients.Feed.Responses;

namespace ShareBook.ClientSdk.Clients.Feed;

public interface IFeedClient
{
	[Get("/feed")]
	Task<ApiResponse<FeedResponse>> Generate([Authorize] string token);
}