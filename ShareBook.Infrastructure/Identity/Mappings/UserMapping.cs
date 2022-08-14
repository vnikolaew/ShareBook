using ShareBook.Domain.Models;
using ShareBook.Domain.Models.User;
using ShareBook.Infrastructure.Common.Persistence.Mappings;

namespace ShareBook.Infrastructure.Identity.Mappings;

public class UserMapping : INodeMapping<User>
{
	public object Map(User entity)
		=> new
		{
			entity.Email,
			entity.Password,
			entity.Username,
			entity.Id
		};
}