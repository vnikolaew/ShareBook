using Neo4jClient.Cypher;

namespace ShareBook.Infrastructure.Common.Extensions;

public static class ICypherFluentQueryExtensions
{
	public static async Task<TResult> GetResult<TResult>(
		this ICypherFluentQuery<TResult> query,
		CancellationToken cancellationToken = default)
		=> (await query.ResultsAsync.WaitAsync(cancellationToken)).SingleOrDefault<TResult>();

	public static async Task<IEnumerable<TResult>> GetResults<TResult>(
		this ICypherFluentQuery<TResult> query,
		CancellationToken cancellationToken = default)
		=> await query.ResultsAsync.WaitAsync(cancellationToken);
}