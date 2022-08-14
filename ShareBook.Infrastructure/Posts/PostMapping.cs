using ShareBook.Domain.Models.Post;
using ShareBook.Infrastructure.Common.Persistence.Mappings;

namespace ShareBook.Infrastructure.Posts;

public class PostMapping : INodeMapping<Post>
{
	public object Map(Post entity)
		=> new { entity.Content, entity.CreatedOn ,entity.ModifiedOn};
}