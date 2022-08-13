using ShareBook.Domain.Common;

namespace ShareBook.Domain.Relationships;
public abstract class Relationship<TFrom, TTo> : Entity<Guid>
  where TFrom : IEntity
  where TTo : IEntity
{
  public TFrom EntityOne { get; protected set; }

  public TTo EntityTwo { get; protected set; }
}