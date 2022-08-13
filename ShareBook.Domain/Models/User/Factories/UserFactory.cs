using ShareBook.Domain.Models.Profile;

namespace ShareBook.Domain.Models.User.Factories;

public class UserFactory : IUserFactory
{
  private Username _username;
  private Email _email;
  private Password _password;
  private Models.Profile.Profile _profile;

  public IUserFactory WithUsername(string username)
    => WithUsername(new Username(username));

  public IUserFactory WithUsername(Username username)
  {
    _username = username;
    return this;
  }

  public IUserFactory WithEmail(string email)
    => WithEmail(new Email(email));

  public IUserFactory WithEmail(Email email)
  {
    _email = email;
    return this;
  }

  public IUserFactory WithPassword(string password)
    => WithPassword(new Password(password));

  public IUserFactory WithPassword(Password password)
  {
    _password = password;
    return this;
  }

  public IUserFactory WithProfile(Models.Profile.Profile profile)
  {
    _profile = profile;
    return this;
  }

  public IUserFactory WithDefaultProfile()
  {
    _profile = new Models.Profile.Profile((FullName) string.Empty, (Bio) string.Empty, Gender.Male);
    return this;
  }

  public User Build()
   => new (_username, _email, _password, _profile);
}