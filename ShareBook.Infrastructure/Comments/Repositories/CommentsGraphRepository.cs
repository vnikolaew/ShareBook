using System.Linq.Expressions;
using Neo4jClient;
using ShareBook.Application.Common.Contracts;
using ShareBook.Domain.Models;
using ShareBook.Domain.Models.Post;
using ShareBook.Domain.Models.User;
using ShareBook.Domain.Relationships;
using ShareBook.Infrastructure.Common.Extensions;
using ShareBook.Infrastructure.Common.Persistence.Mappings;
using ShareBook.Infrastructure.Common.Persistence.Repositories;
using ShareBook.Infrastructure.Common.Reflection;

namespace ShareBook.Infrastructure.Comments.Repositories;

public class CommentsGraphRepository : AuditableGraphRepository<Comment, Guid>
{
	private readonly IAccessorProvider _accessorProvider;
	private static readonly string CommentedLabel = nameof(Commented).ToUpper();
	private const string HasCommentLabel = "HAS_COMMENT";
	
	public CommentsGraphRepository(
		IBoltGraphClient graphClient,
		INodeMapping<Comment> mapper,
		IDateTime dateTime, IAccessorProvider accessorProvider)
		: base(graphClient, mapper, dateTime)
		=> _accessorProvider = accessorProvider;

	public override async Task<Comment> SaveAsync(Comment entity, CancellationToken cancellationToken)
	{
		var query = _graphClient
			.Cypher
			.Match($"(u: {nameof(User)})")
			.Match($"(p: {nameof(Post)})")
			.Where((User u) => u.Id == entity.Author.Id)
			.AndWhere((Post p) => p.Id == entity.Post.Id)
			.With("u, p")
			.Create($"(u)-[r: {CommentedLabel}]->(c: {Label} $comment)<-[cr: {HasCommentLabel}]-(p)")
			.Set($"c.{Id} = apoc.create.uuid()")
			.Set($"c.{CreatedOn} = $now")
			.Set($"c.{ModifiedOn} = $now")
			.WithParams(new { now = _dateTime.Now, comment = _mapper.Map(entity) })
			.Return((u, p, c) => new
			{
				Comment = c.As<Comment>(),
				User = u.As<User>(),
				Post = p.As<Post>()
			});

		var result = await query.GetResult(cancellationToken);
		return PopulateCommentEntity(result.Comment, result.User, result.Post);
	}
	
	private Comment PopulateCommentEntity(
		Comment comment, User user, Post post)
	{
		var userAccessor = _accessorProvider.Get((Comment c) => c.Author);
		var postAccessor = _accessorProvider.Get((Comment c) => c.Post);

		userAccessor[comment] = user;
		postAccessor[comment] = post;
		return comment;
	}
}