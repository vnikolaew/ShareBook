using ShareBook.Domain.Models;
using ShareBook.Infrastructure.Common.Persistence.Mappings;

namespace ShareBook.Infrastructure.Comments;

public class CommentMapping : INodeMapping<Comment>
{
	public object Map(Comment entity)
		=> new {entity.Content};
}