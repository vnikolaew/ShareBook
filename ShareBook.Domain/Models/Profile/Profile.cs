using ShareBook.Domain.Common;

namespace ShareBook.Domain.Models.Profile;
public class Profile : AuditableEntity<Guid>
{
  public FullName FullName { get; private set; }

  public Bio Bio { get; private set; }

  public Media.Media Photo { get; private set; }

  public Gender Gender { get; private set; }

  public Profile(FullName fullName, Bio bio, Gender gender)
  {
    FullName = fullName;
    Bio = bio;
    Gender = gender;
  }

  public Profile() { }

  internal void UpdateBio(Bio bio) => Bio = bio;
  internal void UpdateGender(Gender gender) => Gender = gender;
}