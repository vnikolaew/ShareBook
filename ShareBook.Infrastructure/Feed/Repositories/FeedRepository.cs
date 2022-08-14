using Neo4jClient;
using ShareBook.Domain.Common;
using ShareBook.Domain.Models.Media;
using ShareBook.Domain.Models.Post;
using ShareBook.Domain.Models.User;
using ShareBook.Domain.Relationships;
using ShareBook.Infrastructure.Common.Extensions;
using ShareBook.Infrastructure.Common.Reflection;

namespace ShareBook.Infrastructure.Feed.Repositories;

public class FeedRepository : IFeedRepository
{
	private readonly IBoltGraphClient _graphClient;
	private readonly IAccessorProvider _accessorProvider;

	private const string UserLabel = nameof(User);
	private const string PostLabel = nameof(Post);
	
	private readonly string FollowsRelation = nameof(Follows).ToUpper();
	private readonly string LikedRelation = nameof(Liked).ToUpper();
	
	private const string HasMediaRelation = "HAS_MEDIA";
	private const string CreatedOn = nameof(AuditableEntity<int>.CreatedOn);

	public FeedRepository(
		IBoltGraphClient graphClient,
		IAccessorProvider accessorProvider)
	{
		_graphClient = graphClient;
		_accessorProvider = accessorProvider;
	}

	public async Task<IEnumerable<Post>> GenerateFeedForUser(UserId id, CancellationToken cancellationToken = default)
	{
		var feedQuery = _graphClient
		                .Cypher
		                .Match(
			                $"(u: {UserLabel})-[f: {FollowsRelation}]->(fu: {UserLabel})-[r: POSTED]->(p: {PostLabel})<-[l: {LikedRelation}]-(lu: {UserLabel})")
		                .Where((User u) => u.Id == id)
		                .With("u, lu, fu, l, p")
		                .Match($"(p)-[: {HasMediaRelation}]->(m: {nameof(Media)})")
		                .Return((u, lu, fu, l, p, m) => new
		                {
			                Post = p.As<Post>(),
			                Author = fu.As<User>(),
			                Media = m.As<Media>(),
			                Likes = l.CollectAs<Liked>(),
			                LikedBy = lu.CollectAs<User>()
		                })
		                .OrderByDescending($"p.{CreatedOn}")
		                .Limit(20);

		var results = await feedQuery.GetResults(cancellationToken);
		return results
			.Select(r => PopulatePostEntity(
					r.Post,
					r.Author,
					r.Media,
					r.Likes,
					r.LikedBy));
	}
	
	private Post PopulatePostEntity(
		Post post,
		User author,
		Media media,
		IEnumerable<Liked> likes,
		IEnumerable<User> likedBy)
	{
		var userAccessor = _accessorProvider.Get((Post p) => p.Author);
		userAccessor[post] = author;
		
		var mediaAccessor = _accessorProvider.Get((Post p) => p.Photo);
		mediaAccessor[post] = media;

		likes = likes.Zip(likedBy, (like, user) =>
		{
			var userAccessor = _accessorProvider.Get((Liked l) => l.EntityOne);
			userAccessor[like] = user;
			return like;
		});
		
		var likesAccessor = _accessorProvider.Get((Post p) => p.Likes);
		likesAccessor[post] = likes.ToList();
		
		return post;
	}
}