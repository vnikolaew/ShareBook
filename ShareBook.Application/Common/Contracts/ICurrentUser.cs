namespace ShareBook.Application.Common.Contracts;

public interface ICurrentUser
{
	string? UserId { get; }
	string? UserName { get; }
	IPersonalDetails? Details { get; }
}

public interface IPersonalDetails
{
	string? Id { get; }
	string? Name { get; }
}