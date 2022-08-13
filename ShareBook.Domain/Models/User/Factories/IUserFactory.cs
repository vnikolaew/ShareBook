using ShareBook.Domain.Common;

namespace ShareBook.Domain.Models.User.Factories;

public interface IUserFactory : IFactory<User>
{
  IUserFactory WithUsername(string username);

  IUserFactory WithUsername(Username username);

  IUserFactory WithEmail(string email);

  IUserFactory WithEmail(Email email);

  IUserFactory WithPassword(string password);

  IUserFactory WithPassword(Password password);

  IUserFactory WithProfile(Profile.Profile profile);

  IUserFactory WithDefaultProfile();
}