namespace ShareBook.Application.Likes.Commands.Common;

public class LikeOutputModel
{
	public bool Success { get; set; }
	public IEnumerable<string> Errors { get; set; }
}