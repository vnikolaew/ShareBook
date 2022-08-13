using ShareBook.Domain.Common;
using ShareBook.Domain.Models.User;

namespace ShareBook.Domain.Models.Post.Repositories;

public interface IPostRepository : IDomainRepository<Post, Guid>
{
  Task<IEnumerable<Post>> GetByUserId(UserId userId);
  Task<Post?> FindWithDetails(Guid id, CancellationToken cancellationToken = default);
}