using ShareBook.Domain.Common;
using ShareBook.Domain.Models.User;

namespace ShareBook.Domain.Relationships.Repositories;

public interface ILikeRepository : IRelationshipRepository<Liked, User, Models.Post.Post, UserId, Guid>
{
  Task<IEnumerable<Liked>> GetAllByPostId(Guid id, CancellationToken cancellationToken = default);
}