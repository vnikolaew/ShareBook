namespace ShareBook.Application.Identity.Commands;
public class AuthenticationResult
{
	public bool Succeeded { get; init; }
	public string UserId { get; init; }
	public string Token { get; init; }

	public IEnumerable<string> Errors { get; init; }
		= new List<string>();

	public static AuthenticationResult Success(string userId, string token)
		=> new()
		{
			Succeeded = true,
			UserId = userId,
			Token = token
		};

	public static AuthenticationResult Failure(IEnumerable<string> errors)
		=> new() { Errors = errors };
}