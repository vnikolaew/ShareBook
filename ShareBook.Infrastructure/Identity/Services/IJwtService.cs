using ShareBook.Domain.Models.User;

namespace ShareBook.Infrastructure.Identity.Services;
public interface IJwtService
{
	string GenerateTokenForUser(User user);
}