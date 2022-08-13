namespace ShareBook.Application.Posts.Common;

public class PostsOutputModel
{
	public int Count { get; set; }
	public IEnumerable<PostOutputModel> Posts { get; set; }
}