using ShareBook.Domain.Models.Post;
using ShareBook.Domain.Models.User;

namespace ShareBook.Application.Feed;

public interface IFeedService
{
	Task<IEnumerable<Post>> Generate(UserId id, CancellationToken cancellationToken = default);
}