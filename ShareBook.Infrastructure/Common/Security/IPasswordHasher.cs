namespace ShareBook.Infrastructure.Common.Security;

public interface IPasswordHasher
{
	string Secure(string password);
	bool Verify(string hashedPassword, string providedPassword);	
}