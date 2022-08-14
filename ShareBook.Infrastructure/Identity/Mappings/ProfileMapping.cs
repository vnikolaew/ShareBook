using ShareBook.Domain.Models.Profile;
using ShareBook.Infrastructure.Common.Persistence.Mappings;

namespace ShareBook.Infrastructure.Identity.Mappings;

public class ProfileMapping : INodeMapping<Profile>
{
	public object Map(Profile entity)
		=> new { entity.Bio, entity.Gender, entity.FullName }; 
}