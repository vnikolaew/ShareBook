using Neo4jClient;
using ShareBook.Domain.Common;
using ShareBook.Domain.Relationships;
using ShareBook.Infrastructure.Common.Extensions;
using ShareBook.Infrastructure.Common.Persistence.Mappings;
using ShareBook.Infrastructure.Common.Reflection;

namespace ShareBook.Infrastructure.Common.Persistence.Repositories;
public abstract class GraphRelationshipRepository<TRelationship, TFrom, TTo, TFromId, TToId> : 
	IRelationshipRepository<TRelationship, TFrom, TTo, TFromId, TToId>
		where TRelationship : Relationship<TFrom, TTo>
		where TFrom : Entity<TFromId>
		where TTo : Entity<TToId>
{
	protected readonly IBoltGraphClient _graphClient;
	protected readonly INodeMapping<TRelationship> _relationshipMapper;
	
	protected readonly IAccessorProvider _accessorProvider;
	protected static readonly Accessor<TRelationship, TFrom> FromAccessor = Accessor<TRelationship>.Create(r => r.EntityOne);
	
	protected static readonly string FromLabel = typeof (TFrom).Name;
	protected static readonly string RelationLabel = typeof (TRelationship).Name.ToUpper();
	protected static readonly string ToLabel = typeof (TTo).Name;
	
	protected const string Id = nameof(Entity<int>.Id);

	public GraphRelationshipRepository(
		IBoltGraphClient graphClient,
		INodeMapping<TRelationship> relationshipMapper,
		IAccessorProvider accessorProvider)
	{
		_graphClient = graphClient;
		_relationshipMapper = relationshipMapper;
		_accessorProvider = accessorProvider;
	}

	public async Task<TRelationship> CreateAsync(
		TFromId fromId,
		TToId toId,
		TRelationship relationship,
		CancellationToken cancellationToken = default)
	{
		var query = _graphClient
			.Cypher
			.Match($"(a: {FromLabel})")
			.Match($"(b: {ToLabel})")
			.Where($"a.{Id} = $fromId")
			.AndWhere($"b.{Id} = $toId")
			.With("a, b")
			.Merge($"(a)-[r: {RelationLabel}]->(b)")
			.OnCreate()
			.Set("r += $relation")
			.Set($"r.{Id} = apoc.create.uuid()")
			.WithParams(new
			{
				fromId, toId,
				relation = _relationshipMapper.Map(relationship)
			}).Return((a, b, r) => new
			{
				FromEntity = a.As<TFrom>(),
				ToEntity = a.As<TTo>(),
				Relationship = a.As<TRelationship>(),
			});

		var result = await query.GetResult(cancellationToken: cancellationToken);
		return PopulateRelation(result.Relationship, result.FromEntity, result.ToEntity);
	}

	public async Task<TRelationship> UpdateAsync(
		TFromId fromId,
		TToId toId,
		TRelationship relationship,
		CancellationToken cancellationToken = default)
	{
		var query = _graphClient
			.Cypher
			.Match($"(a: {FromLabel})-[r: {RelationLabel}]->(b: {ToLabel})")
			.Where($"a.{Id} = $fromId")
			.AndWhere($"b.{Id} = $toId")
			.Set("r += $newRelation")
			.WithParams(new { fromId, toId, newRelation = _relationshipMapper.Map(relationship) })
			.Return((a, b, r) => new
			{
				FromEntity = a.As<TFrom>(),
				ToEntity = a.As<TTo>(),
				Relationship = a.As<TRelationship>(),
			});

		var result = await query.GetResult(cancellationToken: cancellationToken);
		return PopulateRelation(result.Relationship, result.FromEntity, result.ToEntity);
	}

	public async Task<bool> DeleteAsync(
		TFromId fromId,
		TToId toId,
		CancellationToken cancellationToken = default)
	{
		var query = _graphClient
		            .Cypher
		            .Match($"(a: {FromLabel})-[r: {RelationLabel}]->(b: {ToLabel})")
		            .Where($"a.{Id} = $fromId")
		            .AndWhere($"b.{Id} = $toId")
		            .WithParams(new { fromId, toId })
		            .Delete("r")
		            .Return<TRelationship>("r");
		
		var result = await query.GetResult(cancellationToken);
		return result is not null;
	}

	public async Task<bool> ExistsAsync(
		TFromId fromId,
		TToId toId,
		CancellationToken cancellationToken = default)
	{
		var query = _graphClient
        		            .Cypher
        		            .Match($"(a: {FromLabel})-[r: {RelationLabel}]->(b: {ToLabel})")
        		            .Where($"a.{Id} = $fromId")
        		            .AndWhere($"b.{Id} = $toId")
        		            .WithParams(new { fromId, toId })
        		            .Return<bool>($"exists((a)-[:{RelationLabel}]->(b))");
		
		return await query.GetResult(cancellationToken);
	}

	private TRelationship PopulateRelation(
		TRelationship relation,
		TFrom from,
		TTo to)
	{
		var fromAccessor = _accessorProvider.Get((TRelationship r) => r.EntityOne);
		var toAccessor = _accessorProvider.Get((TRelationship r) => r.EntityTwo);
		
		fromAccessor[relation] = from;
		toAccessor[relation] = to;
		
		return relation;
	}
}