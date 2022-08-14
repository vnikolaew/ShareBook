using Neo4jClient;
using ShareBook.Application.Common;
using ShareBook.Application.Common.Contracts;
using ShareBook.Domain.Common;
using ShareBook.Domain.Models.Profile;
using ShareBook.Domain.Models.User;
using ShareBook.Domain.Models.User.Repositories;
using ShareBook.Infrastructure.Common.Extensions;
using ShareBook.Infrastructure.Common.Persistence.Mappings;
using ShareBook.Infrastructure.Common.Persistence.Repositories;
using ShareBook.Infrastructure.Common.Reflection;

namespace ShareBook.Infrastructure.Identity.Repositories;

public class GraphUserRepository : GraphRepository<User, UserId>, IUserRepository
{
	private readonly INodeMapping<Profile> _profileMapper;
	private readonly IAccessorProvider _accessorProvider;

	private const string CreatedOn = nameof(AuditableEntity<int>.CreatedOn);
	private const string ModifiedOn = nameof(AuditableEntity<int>.ModifiedOn);
	
	private static readonly string HasProfileRelation = "HAS";

	public GraphUserRepository(
		IBoltGraphClient graphClient,
		INodeMapping<User> mapper,
		IDateTime dateTime,
		INodeMapping<Profile> profileMapper,
		IAccessorProvider accessorProvider)
		: base(graphClient, mapper, dateTime)
	{
		_profileMapper = profileMapper;
		_accessorProvider = accessorProvider;
	}

	public override async Task<User> SaveAsync(User entity, CancellationToken cancellationToken)
	{
		var query = _graphClient
			.Cypher
			.Create($"(u: {Label} $user)-[r: {HasProfileRelation}]->(p: {nameof(Profile)} $profile)")
			.Set($"u.{Id} = apoc.create.uuid()")
			.Set($"p.{Id} = apoc.create.uuid()")
			.Set($"p.{CreatedOn} = $now")
			.Set($"p.{ModifiedOn} = $now")
			.WithParams(new
			{
				user = _mapper.Map(entity),
				profile = _profileMapper.Map(entity.Profile),
				now = _dateTime.Now
			})
			.Return((u, p) => new
			{
				User = u.As<User>(),
				Profile = p.As<Profile>()
			});

		var result = await query.GetResult(cancellationToken);
		var profileAccessor = _accessorProvider.Get((User u) => u.Profile);
		profileAccessor[result.User] = result.Profile;

		return result.User;
	}

	public override async Task<User?> FindAsync(UserId id, CancellationToken cancellationToken)
	{
		var query = _graphClient
        			.Cypher
        			.Match($"(u: {Label})-[r: {HasProfileRelation}]->(p: {nameof(Profile)})")
                    .Where((User u) => u.Id == id)
        			.Return((u, p) => new
        			{
        				User = u.As<User>(),
        				Profile = p.As<Profile>()
        			});
        
		var result = await query.GetResult(cancellationToken);
		var profileAccessor = _accessorProvider.Get((User u) => u.Profile);
		profileAccessor[result.User] = result.Profile;

		return result.User;
	}

	public async Task<User?> FindByEmail(string email, CancellationToken cancellationToken = default)
	{
		var query = _graphClient
		            .Cypher
		            .Match($"(u: {Label})")
		            .Where((User u) => u.Email == email)
		            .Return<User>("u")
		            .Limit(1);
		
		return await query.GetResult(cancellationToken);
	}
}