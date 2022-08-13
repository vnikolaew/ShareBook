using ShareBook.Domain.Common;
using ShareBook.Domain.Models.User;

namespace ShareBook.Domain.Relationships.Repositories;

public interface IFollowRepository : IRelationshipRepository<Follows, User, User, UserId, UserId>
{
  Task<IEnumerable<Follows>> GetFollowsAsync(UserId userId, CancellationToken cancellationToken = default);

  Task<IEnumerable<Follows>> GetFollowersAsync(UserId userId, CancellationToken cancellationToken = default);
}