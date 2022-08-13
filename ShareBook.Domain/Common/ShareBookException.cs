namespace ShareBook.Domain.Common;
public abstract class ShareBookException : Exception
{
	public new string Message { get; init; }
}