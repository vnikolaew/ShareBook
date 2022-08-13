namespace ShareBook.Application.Likes.Queries.Common;

public class LikesOutputModel
{
	public int Count => Likes.Count();
	public IEnumerable<LikeOutputModel> Likes { get; set; }
}