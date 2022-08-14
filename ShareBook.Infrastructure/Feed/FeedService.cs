using ShareBook.Application.Feed;
using ShareBook.Domain.Models.Post;
using ShareBook.Domain.Models.User;
using ShareBook.Infrastructure.Feed.Repositories;

namespace ShareBook.Infrastructure.Feed;

public class FeedService : IFeedService
{
	private readonly IFeedRepository _feed;

	public FeedService(IFeedRepository feed)
		=> _feed = feed;

	public Task<IEnumerable<Post>> Generate(UserId id, CancellationToken cancellationToken = default)
		=> _feed.GenerateFeedForUser(id, cancellationToken);
}