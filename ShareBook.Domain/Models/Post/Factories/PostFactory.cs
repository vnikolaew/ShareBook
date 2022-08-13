namespace ShareBook.Domain.Models.Post.Factories;

public class PostFactory : IPostFactory
{
  private User.User _author;
  private Content _content;
  private Models.Media.Media _media;

  public IPostFactory WithAuthor(User.User author)
  {
    _author = author;
    return this;
  }

  public IPostFactory WithContent(Content content)
  {
    _content = content;
    return this;
  }

  public IPostFactory WithContent(string content)
    => WithContent(new Content(content));

  public IPostFactory WithPhoto(Models.Media.Media photo)
  {
    _media = photo;
    return this;
  }

  public Post Build()
    => new(_author, _content, _media);
}