namespace ShareBook.Application.Common.Contracts;

public interface IUnitOfWork
{
	Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action,
		CancellationToken cancellationToken = default);
}