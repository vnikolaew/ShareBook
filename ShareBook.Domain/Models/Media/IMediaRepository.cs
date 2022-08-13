using ShareBook.Domain.Common;
using ShareBook.Domain.Models.User;

namespace ShareBook.Domain.Models.Media;

public interface IMediaRepository : IDomainRepository<Media, Guid>
{
  Task<Media> SaveProfileMediaAsync(UserId userId, Media media);
  Task<Media> GetByUserIdAsync(UserId userId);
}