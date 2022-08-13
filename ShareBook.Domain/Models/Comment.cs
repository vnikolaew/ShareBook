using ShareBook.Domain.Common;
using ShareBook.Domain.Models.Post;

namespace ShareBook.Domain.Models;
public class Comment : AuditableEntity<Guid>
{
  public Content Content { get; private set; }

  public User.User Author { get; private set; }

  public Post.Post Post { get; private set; }

  public Comment(User.User author, Content content, Post.Post post)
  {
    Author = author;
    Content = content;
    Post = post;
  }

  public Comment() { }

  public void EditContent(Content content) => Content = content;
}