namespace ShareBook.Shared.Abstractions.Queries;

public interface IQueryHandler<in TQuery, TResult>
	where TQuery : IQuery<TResult>
{
	Task<TResult> Handle(TQuery query);
}