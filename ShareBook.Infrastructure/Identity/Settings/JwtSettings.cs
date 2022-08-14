using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ShareBook.Infrastructure.Identity.Settings;
public class JwtSettings
{
	public string AppSecret { get; set; }
	public string ValidIssuer { get; set; }
	public string ValidAudience { get; set; }
	public bool ValidateLifetime { get; set; }
	public bool ValidateActor { get; set; }
	public int RelativeExpirationInHours { get; set; }

	public SymmetricSecurityKey SecurityKey
		=> new(Encoding.UTF8.GetBytes(AppSecret));	
}