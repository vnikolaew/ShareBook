using ShareBook.Domain.Common;
using ShareBook.Domain.Models.Profile.Exceptions;
using static ShareBook.Domain.Models.ModelConstants.Profile;

namespace ShareBook.Domain.Models.Profile;
public class FullName : ValueObject
{
  public string Value { get; }

  public FullName(string value)
  {
    Guard.ForStringLength<InvalidFullNameException>(value, MinFullNameLength, MaxFullNameLength);
    Value = value;
  }

  public static implicit operator string(FullName fullName)
    => fullName.Value;

  public static implicit operator FullName(string fullName)
    => new(fullName);

  public override string ToString() => Value;
}