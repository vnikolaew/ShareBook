using ShareBook.Domain.Common;
using ShareBook.Domain.Models.User.Exceptions;

namespace ShareBook.Domain.Models.User
{
  public class Email : ValueObject
  {
    public string Value { get; }

    public Email(string email)
    {
      Guard.ForValidRegex<InvalidEmailException>(email, ModelConstants.User.ValidEmailRegex);
      Guard.ForStringLength<InvalidEmailException>(email, 5, 200);
      
      Value = email;
    }

    public static implicit operator string(Email email)
      => email.Value;

    public static implicit operator Email(string email)
      => new(email);

    public override string ToString() => Value;
  }
}
