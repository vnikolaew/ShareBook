using Microsoft.Extensions.Logging;
using Neo4jClient.Transactions;
using ShareBook.Application.Common.Contracts;

namespace ShareBook.Infrastructure.Common.Persistence.Repositories;
public class GraphUnitOfWork : IUnitOfWork
{
	private readonly ITransactionalGraphClient _graphClient;
	private readonly ILogger<GraphUnitOfWork> _logger;

	public GraphUnitOfWork(
		ITransactionalGraphClient graphClient,
		ILogger<GraphUnitOfWork> logger)
	{
		_graphClient = graphClient;
		_logger = logger;
	}

	public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action, CancellationToken cancellationToken = default)
	{
		using var tx = _graphClient.BeginTransaction(TransactionScopeOption.RequiresNew);
		try
		{
			var result = await action();
			await tx.CommitAsync();
			
			return result;
		}
		catch (Exception e)
		{
			_logger.LogInformation("Error during database transaction: {Message}", e.Message);
			await tx.RollbackAsync();
			
			throw;
		}
	}
}