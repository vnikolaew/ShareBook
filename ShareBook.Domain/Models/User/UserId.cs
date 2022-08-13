using ShareBook.Domain.Common;
using ShareBook.Domain.Models.User.Exceptions;

namespace ShareBook.Domain.Models.User;
public class UserId : ValueObject
{
  public Guid Value { get; }

  public UserId(Guid value)
  {
    if (value == Guid.Empty)
    {
      throw new InvalidUsernameException($"{value} cannot be empty.");
    }
    
    Value = value;
  }

  public static implicit operator Guid(UserId userId)
    => userId.Value;

  public static implicit operator UserId(Guid id)
    => new(id);

  public static implicit operator UserId(string id)
    => new(Guid.Parse(id));

  public static implicit operator string(UserId userId)
    => userId.Value.ToString();

  public override string ToString()
    => Value.ToString();
}