using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Common;

public abstract class EntityCommand<TId> : IRequest
{
	public TId Id { get; set; }
}