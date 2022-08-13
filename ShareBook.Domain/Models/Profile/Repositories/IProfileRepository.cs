using ShareBook.Domain.Common;
using ShareBook.Domain.Models.User;

namespace ShareBook.Domain.Models.Profile.Repositories;

public interface IProfileRepository : IDomainRepository<Profile, Guid>
{
  Task<Profile?> GetByUserId(UserId id, CancellationToken cancellationToken = default);
}