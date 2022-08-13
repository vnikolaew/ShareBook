namespace ShareBook.Application.Follows.Queries.Common;
public class FollowersOutputModel
{
	public int Count { get; set; }
	public IEnumerable<FollowerOutputModel> Followers { get; set; }
}