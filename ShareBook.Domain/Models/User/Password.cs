using ShareBook.Domain.Common;

namespace ShareBook.Domain.Models.User;

public class Password : ValueObject
{
  public string Value { get; }

  public Password(string password)
    => Value = password;

  public static implicit operator string(Password password)
    => password.Value;

  public static implicit operator Password(string password)
    => new(password);

  public override string ToString()
    => Value;
}