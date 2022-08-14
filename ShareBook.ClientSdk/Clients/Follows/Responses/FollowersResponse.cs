namespace ShareBook.ClientSdk.Clients.Follows.Responses;

public class FollowersResponse
{
	public int Count { get; set; }
	public IEnumerable<FollowerResponseModel> Followers { get; set; }
}