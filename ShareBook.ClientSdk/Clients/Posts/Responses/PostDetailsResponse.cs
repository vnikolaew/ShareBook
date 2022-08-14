namespace ShareBook.ClientSdk.Clients.Posts.Responses;

public class PostDetailsResponse : PostResponseModel
{
	public string Author { get; set; }
	public IEnumerable<LikeResponseModel> Likes { get; set; }
	public IEnumerable<CommentResponseModel> Comments { get; set; }
}

public class CommentResponseModel
{
	public string Id { get; set; }
	public string Author { get; set; }
	public DateTime CreatedOn { get; set; }
	public string Content { get; set; }
}

public class LikeResponseModel
{
	public string Id { get; set; }
	public string UserId { get; set; }
	public string Username { get; set; }
	public DateTime At { get; set; }
}