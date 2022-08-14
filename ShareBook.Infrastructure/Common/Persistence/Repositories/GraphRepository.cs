using Neo4jClient;
using ShareBook.Application.Common.Contracts;
using ShareBook.Domain.Common;
using ShareBook.Infrastructure.Common.Extensions;
using ShareBook.Infrastructure.Common.Persistence.Mappings;

namespace ShareBook.Infrastructure.Common.Persistence.Repositories;

public abstract class GraphRepository<TEntity, TId> : IDomainRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : notnull
{
  protected readonly IBoltGraphClient _graphClient;
  
  protected static readonly string Label = typeof (TEntity).Name;
  protected const string Id = nameof(Entity<int>.Id);
  
  protected readonly INodeMapping<TEntity> _mapper;
  protected readonly IDateTime _dateTime;

  protected GraphRepository(
    IBoltGraphClient graphClient,
    INodeMapping<TEntity> mapper,
    IDateTime dateTime)
  {
    _graphClient = graphClient;
    _mapper = mapper;
    _dateTime = dateTime;
  }

  public async Task<IEnumerable<TEntity>> All()
  {
    var query = _graphClient
                .Cypher
                .Match($"(e: {Label})")
                .Return<IEnumerable<TEntity>>("e");

    return await query.GetResult();
  }

  public virtual async Task<TEntity?> FindAsync(TId id, CancellationToken cancellationToken)
  {
    var query = _graphClient
                .Cypher
                .Match($"(e: {Label})")
                .Where($"e.{Id} = $id")
                .WithParam(nameof(id), id)
                .Return<TEntity>("e");
    
    return await query.GetResult(cancellationToken);
  }

  public virtual async Task<TEntity> SaveAsync(
    TEntity entity,
    CancellationToken cancellationToken)
  {
    var query = _graphClient
                .Cypher
                .Create($"(e: {Label} $entity)")
                .WithParam("entity", _mapper.Map(entity))
                .Set($"e.{Id} = apoc.create.uuid()")
                .Return<TEntity>("e");

    return await query.GetResult(cancellationToken);
  }

  public virtual async Task<TEntity> UpdateAsync(
    TEntity entity,
    CancellationToken cancellationToken)
  {
    var query = _graphClient
                .Cypher
                .Match($"(e: {Label})")
                .Where($"e.{Id} = $id")
                .Set("e += $newEntity")
                .WithParams(new {id = entity.Id, newEntity = _mapper.Map(entity)})
                .Return<TEntity>("e");

    return await query.GetResult(cancellationToken);
  }

  public virtual async Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken)
  {
    var query = _graphClient
                .Cypher
                .Match($"(e: {Label})")
                .Where($"e.{Id} = $id")
                .WithParam("id", id)
                .DetachDelete("e")
                .Return<TEntity>("e");

    return await query.GetResult(cancellationToken) != null;
  }
}
