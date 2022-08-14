using Neo4jClient;
using ShareBook.Application.Common.Contracts;
using ShareBook.Domain.Models.Media;
using ShareBook.Domain.Models.Profile;
using ShareBook.Domain.Models.Profile.Repositories;
using ShareBook.Domain.Models.User;
using ShareBook.Infrastructure.Common.Extensions;
using ShareBook.Infrastructure.Common.Persistence.Mappings;
using ShareBook.Infrastructure.Common.Persistence.Repositories;
using ShareBook.Infrastructure.Common.Reflection;

namespace ShareBook.Infrastructure.Identity.Repositories;

public class GraphProfileRepository :
	AuditableGraphRepository<Profile, Guid>, IProfileRepository
{
	private const string HasProfileRelation = "HAS";
	private const string HasMediaRelation = "HAS_MEDIA";
	
	private readonly IAccessorProvider _accessorProvider;

	public GraphProfileRepository(
		IBoltGraphClient graphClient,
		INodeMapping<Profile> mapper,
		IDateTime dateTime, IAccessorProvider accessorProvider)
		: base(graphClient, mapper, dateTime)
		=> _accessorProvider = accessorProvider;

	public async Task<Profile?> GetByUserId(UserId id, CancellationToken cancellationToken = default)
	{
		var query = _graphClient
		            .Cypher
		            .Match(
			            $"(u: {nameof(User)})-[: {HasProfileRelation}]->(p: {Label})-[hm: {HasMediaRelation}]->(m: {nameof(Media)})")
		            .Where((User u) => u.Id == id)
		            .Return((p, m) => new
		            {
			            Profile = p.As<Profile>(),
			            Photo = m.As<Media>()
		            });

		var result = await query.GetResult(cancellationToken);
		
		var mediaAccessor = _accessorProvider.Get((Profile p) => p.Photo);
		mediaAccessor[result.Profile] = result.Photo;

		return result.Profile;
	}
}