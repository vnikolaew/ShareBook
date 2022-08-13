namespace ShareBook.Domain.Common;

public interface IAuditableEntity : IEntity
{
  DateTime CreatedOn { get; }

  DateTime ModifiedOn { get; }
}