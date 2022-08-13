﻿namespace ShareBook.Domain.Common;
public interface IEntity
{
  IReadOnlyCollection<IDomainEvent> Events { get; }

  void ClearEvents();
}