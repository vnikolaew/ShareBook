using ShareBook.Application.Identity.Commands;
using ShareBook.Application.Identity.Commands.SignUp;
using ShareBook.Application.Identity.Queries.Authenticate;
using ShareBook.Domain.Models.User;

namespace ShareBook.Application.Identity;
public interface IIdentityService
{
  Task<AuthenticationResult> SignUpAsync(SignUpRequestModel requestModel);
  Task<AuthenticationResult> Authenticate(AuthRequestModel authRequestModel);

  Task<User?> FindById(UserId userId,
    CancellationToken cancellationToken = default);
}