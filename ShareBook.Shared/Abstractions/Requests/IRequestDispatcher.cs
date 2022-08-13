namespace ShareBook.Shared.Abstractions.Requests;

public interface IRequestDispatcher
{

	Task<TResult> Dispatch<TRequest, TResult>(
		TRequest request, CancellationToken cancellationToken = default)
			where TRequest : IRequest<TResult>;
	
	Task Dispatch<TRequest>(
		TRequest request, CancellationToken cancellationToken = default)
			where TRequest : IRequest;
}