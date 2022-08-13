using ShareBook.Domain.Models.User;

namespace ShareBook.Domain.Relationships;

public class Follows : Relationship<User, User>
{
  public DateTime Since { get; private set; }

  public Follows(DateTime since) => Since = since;

  public Follows() { }
}