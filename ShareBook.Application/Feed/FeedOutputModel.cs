namespace ShareBook.Application.Feed;
public class FeedOutputModel
{
	public int Count { get; set; }
	public IEnumerable<FeedPostOutputModel> Posts { get; set; }
}