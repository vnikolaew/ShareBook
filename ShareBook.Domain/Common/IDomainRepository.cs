namespace ShareBook.Domain.Common;

public interface IDomainRepository<TEntity, in TId>
  where TEntity : Entity<TId>
  where TId : notnull
{
  Task<TEntity?> FindAsync(TId id, CancellationToken cancellationToken = default (CancellationToken));

  Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default (CancellationToken));

  Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken = default (CancellationToken));

  Task<TEntity> SaveAsync(TEntity entity, CancellationToken cancellationToken = default (CancellationToken));
}