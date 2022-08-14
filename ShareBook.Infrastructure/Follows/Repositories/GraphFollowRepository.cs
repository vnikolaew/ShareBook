using AutoMapper;
using Neo4jClient;
using ShareBook.Domain.Models.User;
using ShareBook.Domain.Relationships.Repositories;
using ShareBook.Infrastructure.Common.Extensions;
using ShareBook.Infrastructure.Common.Persistence.Mappings;
using ShareBook.Infrastructure.Common.Persistence.Repositories;
using ShareBook.Infrastructure.Common.Reflection;
using DomainFollows = ShareBook.Domain.Relationships.Follows;

namespace ShareBook.Infrastructure.Follows.Repositories;

public class GraphFollowRepository :
	GraphRelationshipRepository<DomainFollows, User, User, UserId, UserId>,
	IFollowRepository
{
	public GraphFollowRepository(
		IBoltGraphClient graphClient,
		INodeMapping<DomainFollows> relationshipMapper,
		 IAccessorProvider accessorProvider)
		: base(graphClient, relationshipMapper, accessorProvider) { }

	public async Task<IEnumerable<DomainFollows>> GetFollowsAsync(UserId userId, CancellationToken cancellationToken = default)
	{
		var query = _graphClient
			.Cypher
			.Match($"(u: {FromLabel})-[r: {RelationLabel}]->(f: {ToLabel})")
			.Where((User u) => u.Id == userId)
			.With($"u, r, f")
			.OptionalMatch($"(f)-[:HAS]->(p: {nameof(Profile)})")
			.Return((u, r, f, p) => new
			{
				Follows = r.CollectAs<DomainFollows>(),
				Following = f.CollectAs<User>(),
				Profiles = p.CollectAs<Domain.Models.Profile.Profile>()
			});

		var result = await query.GetResult(cancellationToken);
		return PopulateFollowEntities(result.Follows, result.Following, result.Profiles);
	}

	public async Task<IEnumerable<DomainFollows>> GetFollowersAsync(UserId userId, CancellationToken cancellationToken = default)
	{
		var query = _graphClient
			.Cypher
			.Match($"(u: {FromLabel})<-[r: {RelationLabel}]-(f: {ToLabel})")
			.Where((User u) => u.Id == userId)
			.With("u, r, f")
			.OptionalMatch($"(f)-[:HAS]->(p: {nameof(Profile)})")
			.Return((u, r, f, p) => new
			{
				Follows = r.CollectAs<DomainFollows>(),
				Following = f.CollectAs<User>(),
				Profiles = p.CollectAs<Domain.Models.Profile.Profile>()
			});

		var result = await query.GetResult(cancellationToken);
		return PopulateFollowEntities(result.Follows, result.Following, result.Profiles, false);
	}

	private IEnumerable<DomainFollows> PopulateFollowEntities(
		IEnumerable<DomainFollows> follows,
		IEnumerable<User> users,
		IEnumerable<Domain.Models.Profile.Profile> profiles,
		bool asFollower = true)
	{
		var followeeAccessor = _accessorProvider.Get<DomainFollows, User>(f => f.EntityTwo);
		var followerAccessor = _accessorProvider.Get((DomainFollows f) => f.EntityOne);
		
		var profileAccessor = _accessorProvider.Get((User u) => u.Profile);
		
		var userEnumerator = users.GetEnumerator();
		var profileEnumerator = profiles.GetEnumerator();
		
		foreach (var follow in follows)
		{
			userEnumerator.MoveNext();
			profileEnumerator.MoveNext();
			
			(asFollower ? followeeAccessor : followerAccessor)[follow] = userEnumerator.Current;
			profileAccessor[userEnumerator.Current] = profileEnumerator.Current;
		}
		
		return follows;
	}
}