using ShareBook.Domain.Common;
using ShareBook.Domain.Relationships;

namespace ShareBook.Domain.Models.Post;
public class Post : AuditableEntity<Guid>, IAggregateRoot
{
  private ICollection<Liked> _likes = new List<Liked>();
  
  private ICollection<Comment> _comments = new List<Comment>();

  public Content Content { get; private set; }
  public User.User Author { get; private set; }

  public Media.Media Photo { get; private set; }

  public Post(User.User author, Content content, Media.Media photo)
  {
    Content = content;
    Author = author;
    Photo = photo;
  }

  public Post() { }

  public ICollection<Liked> Likes
  {
    get => _likes.ToList().AsReadOnly();
    private set => _likes = value;
  }

  public ICollection<Comment> Comments
  {
    get => _comments.ToList().AsReadOnly();
    private set => _comments = value;
  }

  public void EditContent(Content content)
    => Content = content;
}