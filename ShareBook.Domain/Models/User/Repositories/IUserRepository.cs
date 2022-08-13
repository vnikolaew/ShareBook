using ShareBook.Domain.Common;

namespace ShareBook.Domain.Models.User.Repositories;

public interface IUserRepository : IDomainRepository<User, UserId>
{
  Task<User?> FindByEmail(string email, CancellationToken cancellationToken = default);
}