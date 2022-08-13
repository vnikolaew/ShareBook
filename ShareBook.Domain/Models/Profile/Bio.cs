using ShareBook.Domain.Common;
using ShareBook.Domain.Models.Profile.Exceptions;
using static ShareBook.Domain.Models.ModelConstants.Profile;

namespace ShareBook.Domain.Models.Profile;
public class Bio : ValueObject
{
  public string Value { get; }

  public Bio(string value)
  {
    Guard.ForStringLength<BioLengthOutOfRangeException>(value, MinBioLength, MaxBioLength);
    Value = value;
  }

  public static implicit operator string(Bio bio)
    => bio.Value;

  public static implicit operator Bio(string bio)
    => new(bio);

  public override string ToString() => Value;
}