namespace ShareBook.Domain.Common;
public abstract class Entity<TId> : IEntity where TId : notnull
{
  private readonly ICollection<IDomainEvent> _events
    = new List<IDomainEvent>();

  public TId Id { get; protected set; }

  public IReadOnlyCollection<IDomainEvent> Events
    => _events.ToList().AsReadOnly();

  public void ClearEvents()
    => _events.Clear();

  public void RaiseEvent(IDomainEvent @event)
    => _events.Add(@event);

  public override bool Equals(object? obj)
  {
    var entity = obj as Entity<TId>;

    if ((object) entity == null)
    {
      return false;
    }

    if (this == entity)
    {
      return true;
    }
    
    return !(GetType() != entity.GetType())
           && !Id.Equals(null)
           && !entity.Id.Equals(null)
           && Id.Equals(entity.Id);
  }

  public static bool operator ==(Entity<TId>? first, Entity<TId>? second)
  {
    if ((object) first == null && (object) second == null)
    {
      return true;
    }

    if (first is null || second is null)
    {
      return false;
    }
      
    return first.Equals(second);
  }

  public static bool operator !=(Entity<TId> first, Entity<TId> second)
    => !(first == second);

  public override int GetHashCode() => Id.GetHashCode();
}