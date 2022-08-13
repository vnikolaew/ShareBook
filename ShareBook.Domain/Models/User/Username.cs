using ShareBook.Domain.Common;
using ShareBook.Domain.Models.User.Exceptions;

namespace ShareBook.Domain.Models.User;
public class Username : ValueObject
{
  public string Value { get; }

  public Username(string username)
  {
    Guard.ForStringLength<InvalidUsernameException>(username, 3, 50);
    Value = username;
  }

  public static implicit operator string(Username username)
    => username.Value;

  public static implicit operator Username(string username)
    => new(username);

  public override string ToString() => Value;
}