using ShareBook.Domain.Relationships;
using ShareBook.Infrastructure.Common.Persistence.Mappings;

namespace ShareBook.Infrastructure.Likes;

public class LikesMapping : INodeMapping<Liked>
{
	public object Map(Liked entity)
		=> new { entity.CreatedOn };
}