namespace ShareBook.Application.Common.Contracts;
public interface ICurrentUser
{
	string? UserId { get; }
	string? UserName { get; }
}