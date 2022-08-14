using ShareBook.ClientSdk.Clients.Posts.Responses;

namespace ShareBook.ClientSdk.Clients.Follows.Responses;
public class FollowerResponseModel
{
	public ICollection<PostResponseModel> Posts = new List<PostResponseModel>();
	public string Id { get; set; }
	public string Username { get; set; }
	public string Email { get; set; }
	public DateTime Since { get; set; }
}