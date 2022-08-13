namespace ShareBook.Application.Follows.Queries.Common;

public class FollowingOutputModel
{
	public int Count { get; set; }
	public IEnumerable<FolloweeOutputModel> Following { get; set; }
}