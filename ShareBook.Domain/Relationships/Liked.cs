using ShareBook.Domain.Models.Post;
using ShareBook.Domain.Models.User;

namespace ShareBook.Domain.Relationships;

public class Liked : Relationship<User, Post>
{
  public DateTime CreatedOn { get; private set; }

  public Liked(DateTime createdOn) => CreatedOn = createdOn;

  public Liked() { }
}