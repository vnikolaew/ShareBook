using ShareBook.Domain.Models.Post;
using ShareBook.Domain.Models.User;

namespace ShareBook.Infrastructure.Feed.Repositories;

public interface IFeedRepository
{
	Task<IEnumerable<Post>> GenerateFeedForUser(UserId id, CancellationToken cancellationToken = default);
}