using Neo4jClient;
using ShareBook.Application.Common.Contracts;
using ShareBook.Domain.Common;
using ShareBook.Infrastructure.Common.Extensions;
using ShareBook.Infrastructure.Common.Persistence.Mappings;

namespace ShareBook.Infrastructure.Common.Persistence.Repositories;

public abstract class AuditableGraphRepository<TEntity, TId> : GraphRepository<TEntity, TId>
	where TEntity : AuditableEntity<TId>
{
	protected const string CreatedOn = nameof(AuditableEntity<int>.CreatedOn);
	protected const string ModifiedOn = nameof(AuditableEntity<int>.ModifiedOn);
	
	public AuditableGraphRepository(
		IBoltGraphClient graphClient,
		INodeMapping<TEntity> mapper,
		IDateTime dateTime)
		: base(graphClient, mapper, dateTime) { }
	
	public override async Task<TEntity> SaveAsync(
		TEntity entity,
		CancellationToken cancellationToken)
	{
		var query = _graphClient
		            .Cypher
		            .Create($"(e: {Label} $entity)")
		            .Set($"e.{Id} = apoc.create.uuid()")
		            .Set($"e.{CreatedOn} = $now")
		            .Set($"e.{ModifiedOn} = $now")
		            .WithParams(new {entity = _mapper.Map(entity), now = _dateTime.Now})
		            .Return<TEntity>("e");

		return await query.GetResult(cancellationToken);
	}

	public override async Task<TEntity> UpdateAsync(
		TEntity entity,
		CancellationToken cancellationToken)
	{
		var query = _graphClient
		            .Cypher
		            .Match($"(e: {Label} $entity)")
		            .Where($"e.{Id} = $id")
		            .Set("e += $newEntity")
		            .Set($"e.{ModifiedOn} = $now")
		            .WithParams(new
		            {
			            id = entity.Id,
			            newEntity = _mapper.Map(entity),
			            now = _dateTime.Now,
		            })
		            .Return<TEntity>("e");

		return await query.GetResult(cancellationToken);
	}
}