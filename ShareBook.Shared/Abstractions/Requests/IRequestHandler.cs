namespace ShareBook.Shared.Abstractions.Requests;

public interface IRequestHandler<in TRequest>
	where TRequest : IRequest
{
	Task Handle(TRequest request, CancellationToken cancellationToken = default);
}

public interface IRequestHandler<in TRequest, TResult>
	where TRequest : IRequest<TResult>
{
	Task<TResult> Handle(TRequest request, CancellationToken cancellationToken = default);
}
