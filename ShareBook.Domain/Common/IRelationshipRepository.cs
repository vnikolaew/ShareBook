using ShareBook.Domain.Relationships;

namespace ShareBook.Domain.Common;

public interface IRelationshipRepository<TRelationship, TFrom, TTo, in TFromId, in TToId>
  where TRelationship : Relationship<TFrom, TTo>
  where TFrom : IEntity
  where TTo : IEntity
{
  Task<TRelationship> CreateAsync(
    TFromId fromId,
    TToId toId,
    TRelationship relationship,
    CancellationToken cancellationToken = default (CancellationToken));

  Task<TRelationship> UpdateAsync(
    TFromId fromId,
    TToId toId,
    TRelationship relationship,
    CancellationToken cancellationToken = default (CancellationToken));

  Task<bool> DeleteAsync(TFromId fromId, TToId toId, CancellationToken cancellationToken = default (CancellationToken));

  Task<bool> ExistsAsync(TFromId fromId, TToId toId, CancellationToken cancellationToken = default (CancellationToken));
}