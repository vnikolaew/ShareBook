using ShareBook.Domain.Models.Media;
using ShareBook.Infrastructure.Common.Persistence.Mappings;

namespace ShareBook.Infrastructure.Identity.Mappings;
public class MediaMapping : INodeMapping<Media>
{
	public object Map(Media entity)
		=> new { entity.MediaName, entity.MediaType, entity.AbsoluteUrl };
}