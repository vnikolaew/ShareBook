namespace ShareBook.Application.Posts.Commands.Create;
public class PostMediaModel
{
	public Stream Content { get; set; }
	public string ContentType { get; set; }
	public string MediaName { get; set; }
}