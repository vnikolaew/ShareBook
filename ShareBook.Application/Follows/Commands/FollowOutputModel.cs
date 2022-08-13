namespace ShareBook.Application.Follows.Commands;
public class FollowOutputModel
{
	public bool Success { get; set; }
	public IEnumerable<string> Errors { get; set; }
}