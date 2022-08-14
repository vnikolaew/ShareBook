using ShareBook.ClientSdk.Clients.Posts.Responses;
using LikeResponseModel = ShareBook.ClientSdk.Clients.Posts.Responses.LikeResponseModel;

namespace ShareBook.ClientSdk.Clients.Feed.Responses;

public class FeedResponse
{
	public int Count { get; set; }
	public IEnumerable<FeedPostResponseModel> Posts { get; set; }
}

public class FeedPostResponseModel
{
	public string Id { get; set; }
	public string Author { get; set; }
	public DateTime CreatedOn { get; set; }
	public DateTime ModifiedOn { get; set; }
	public string Content { get; set; }
	public PostResponseModel Media { get; set; }
	public IEnumerable<LikeResponseModel> Likes { get; set; }
	public IEnumerable<CommentResponseModel> Comments { get; set; }
}