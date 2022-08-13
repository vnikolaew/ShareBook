namespace ShareBook.Domain.Common;

public abstract class AuditableEntity<TId>
  : Entity<TId>, IAuditableEntity
{
  public DateTime CreatedOn { get; protected set; }

  public DateTime ModifiedOn { get; protected set; }
}