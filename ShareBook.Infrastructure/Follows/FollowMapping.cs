using ShareBook.Infrastructure.Common.Persistence.Mappings;

namespace ShareBook.Infrastructure.Follows;

public class FollowMapping : INodeMapping<Domain.Relationships.Follows>
{
	public object Map(Domain.Relationships.Follows entity)
		=> new { entity.Since };
}