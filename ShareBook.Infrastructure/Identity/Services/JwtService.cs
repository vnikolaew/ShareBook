using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using ShareBook.Domain.Models.User;
using ShareBook.Infrastructure.Identity.Settings;

namespace ShareBook.Infrastructure.Identity.Services;
public class JwtService : IJwtService
{
	private readonly JwtSettings _jwtSettings;

	public JwtService(JwtSettings jwtSettings)
		=> _jwtSettings = jwtSettings;
	public string GenerateTokenForUser(User user)
	{
		var credentials = new SigningCredentials(_jwtSettings.SecurityKey,
			SecurityAlgorithms.HmacSha256);

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new List<Claim>
			{
				new(ClaimTypes.Email, user.Email),
				new(ClaimTypes.NameIdentifier, user.Id),
				new(ClaimTypes.Name, user.Username),
			}),
			Expires = DateTime.Now.AddHours(_jwtSettings.RelativeExpirationInHours),
			SigningCredentials = credentials,
			Issuer = _jwtSettings.ValidIssuer,
			Audience = _jwtSettings.ValidAudience,
			IssuedAt = DateTime.Now
		};
		
		return GetTokenFromDescriptor(tokenDescriptor);
	}
	
	private static string GetTokenFromDescriptor(SecurityTokenDescriptor descriptor)
	{
		var securityTokenHandler = new JwtSecurityTokenHandler();
		return securityTokenHandler.WriteToken(securityTokenHandler.CreateToken(descriptor));
	}
}