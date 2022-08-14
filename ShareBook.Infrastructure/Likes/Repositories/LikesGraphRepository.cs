using Neo4jClient;
using ShareBook.Domain.Common;
using ShareBook.Domain.Models.Post;
using ShareBook.Domain.Models.User;
using ShareBook.Domain.Relationships;
using ShareBook.Domain.Relationships.Repositories;
using ShareBook.Infrastructure.Common.Extensions;
using ShareBook.Infrastructure.Common.Persistence.Mappings;
using ShareBook.Infrastructure.Common.Persistence.Repositories;
using ShareBook.Infrastructure.Common.Reflection;

namespace ShareBook.Infrastructure.Likes.Repositories;

public class LikesGraphRepository : 
	GraphRelationshipRepository<Liked, User, Post, UserId, Guid>,
	ILikeRepository
{
	private const string CreatedOn = nameof(AuditableEntity<int>.CreatedOn);
	
	public LikesGraphRepository(
		IBoltGraphClient graphClient,
		INodeMapping<Liked> relationshipMapper,
		IAccessorProvider accessorProvider)
		: base(graphClient, relationshipMapper, accessorProvider) { }

	public async Task<IEnumerable<Liked>> GetAllByPostId(
		Guid id,
		CancellationToken cancellationToken = default)
	{
		var query = _graphClient
			.Cypher
			.Match($"(p: {ToLabel})<-[r: {RelationLabel}]-(u: {FromLabel})")
			.Where((Post p) => p.Id == id)
			.OrderByDescending($"r.{CreatedOn}")
			.Return((p, r, u) => new
			{
				Post = p.As<Post>(),
				LikedBy = u.CollectAs<User>(),
				Liked = r.CollectAs<Liked>()
			});
		
		var result = await query.GetResult(cancellationToken);
		return result.Liked.Zip(result.LikedBy, PopulateLikeEntity);
	}

	private Liked PopulateLikeEntity(Liked like, User user)
	{
		var userAccessor = _accessorProvider.Get((Liked l) => l.EntityOne);
		userAccessor[like] = user;
		return like;
	}
}