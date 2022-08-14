namespace ShareBook.ClientSdk.Clients.Posts.Responses;

public class PostsResponse
{
	public int Count { get; set; }
	public IEnumerable<PostResponseModel> Posts { get; set; }
}

public class PostResponseModel
{
	public string Id { get; set; }
	public DateTime CreatedOn { get; set; }
	public DateTime LastModifiedOn { get; set; }
	public string Content { get; set; }
	public PostPhotoResponseModel Media { get; set; }
}

public class PostPhotoResponseModel
{
	public string Id { get; set; }
	public string PhotoUrl { get; set; }
}