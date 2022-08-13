namespace ShareBook.Shared.Abstractions.Requests;
public interface IRequest
{
}

public interface IRequest<TResult> : IRequest
	where TResult : notnull
{
}
