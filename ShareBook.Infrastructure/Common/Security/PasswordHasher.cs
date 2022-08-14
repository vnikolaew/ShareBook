using Microsoft.AspNetCore.Identity;
using ShareBook.Domain.Models.User;

namespace ShareBook.Infrastructure.Common.Security;
public class PasswordHasher : IPasswordHasher
{
	private readonly IPasswordHasher<User> _internalHasher;

	public PasswordHasher(IPasswordHasher<User> internalHasher)
		=> _internalHasher = internalHasher;

	public string Secure(string password)
		=> _internalHasher.HashPassword(null, password);

	public bool Verify(string hashedPassword, string providedPassword)
		=> _internalHasher
			   .VerifyHashedPassword(null, hashedPassword, providedPassword)
		   == PasswordVerificationResult.Success;
}