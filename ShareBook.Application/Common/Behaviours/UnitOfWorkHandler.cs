using ShareBook.Application.Common.Contracts;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Common.Behaviours;

public class UnitOfWorkHandler<TRequest, TResult>
	: IRequestHandler<TRequest, TResult>
	where TRequest : IRequest<TResult>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IRequestHandler<TRequest, TResult> _inner;

	public UnitOfWorkHandler(
		IUnitOfWork unitOfWork,
		IRequestHandler<TRequest, TResult> inner)
	{
		_unitOfWork = unitOfWork;
		_inner = inner;
	}

	public Task<TResult> Handle(TRequest request, CancellationToken cancellationToken = default)
		=> _unitOfWork.ExecuteAsync(() => _inner.Handle(request, cancellationToken), cancellationToken);
}