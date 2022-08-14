namespace ShareBook.ClientSdk.Clients.Likes.Responses;

public class LikesResponse 
{
	public int Count { get; set; }
	public IEnumerable<LikeResponseModel> Likes { get; set; }
}

public class LikeResponseModel
{
	public string Id { get; set; }
	public string UserId { get; set; }
	public string Username { get; set; }
	public DateTime At { get; set; }
}