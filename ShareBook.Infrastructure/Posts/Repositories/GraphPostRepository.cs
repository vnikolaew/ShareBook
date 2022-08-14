using Neo4jClient;
using ShareBook.Application.Common.Contracts;
using ShareBook.Domain.Models;
using ShareBook.Domain.Models.Media;
using ShareBook.Domain.Models.Post;
using ShareBook.Domain.Models.Post.Repositories;
using ShareBook.Domain.Models.User;
using ShareBook.Domain.Relationships;
using ShareBook.Infrastructure.Common.Extensions;
using ShareBook.Infrastructure.Common.Persistence.Mappings;
using ShareBook.Infrastructure.Common.Persistence.Repositories;
using ShareBook.Infrastructure.Common.Reflection;

namespace ShareBook.Infrastructure.Posts.Repositories;

public class GraphPostRepository :
	AuditableGraphRepository<Post, Guid>, IPostRepository
{
	private readonly INodeMapping<Media> _mediaMapper;
	private readonly IAccessorProvider _accessorProvider;
	
	private const string PostedRelation = "POSTED";
	private const string HasMediaRelation = "HAS_MEDIA";
	
	private static readonly string LikedRelation = nameof(Liked).ToUpper();
	private static readonly string CommentedRelation = nameof(Commented).ToUpper();
	
	public GraphPostRepository(
		IBoltGraphClient graphClient,
		INodeMapping<Post> mapper,
		IDateTime dateTime,
		INodeMapping<Media> mediaMapper,
		IAccessorProvider accessorProvider)
		: base(graphClient, mapper, dateTime)
	{
		_mediaMapper = mediaMapper;
		_accessorProvider = accessorProvider;
	}

	public override async Task<Post> SaveAsync(Post entity, CancellationToken cancellationToken)
	{
		var query = _graphClient
			.Cypher
			.Match($"(u: {nameof(User)})")
			.Where((User u) => u.Id == entity.Author.Id)
			.Create($"(u)-[r: {PostedRelation}]->(p: {Label} $post)-[: {HasMediaRelation}]->(m: {nameof(Media)} $media)")
			.Set($"p.{Id} = apoc.create.uuid()")
			.Set($"p.{CreatedOn} = $now, p.{ModifiedOn} = $now")
			.Set($"m.{Id} = apoc.create.uuid()")
			.Set($"m.{CreatedOn} = $now, m.{ModifiedOn} = $now")
			.WithParams(new
			{
				now = _dateTime.Now,
				post = _mapper.Map(entity),
				media = _mediaMapper.Map(entity.Photo)
			})
			.Return((u, p, m) => new
			{
				User = u.As<User>(),
				Post = p.As<Post>(),
				Media = m.As<Media>()
			});
		
		var result = await query.GetResult(cancellationToken);
		return PopulatePostEntity(result.Post, result.User, result.Media);
	}

	public override async Task<Post?> FindAsync(Guid id, CancellationToken cancellationToken)
	{
		var query = _graphClient
			.Cypher
			.Match($"(m: {nameof(Media)})<-[:{HasMediaRelation}]-(p: {Label})<-[r: {PostedRelation}]-(u: {nameof(User)})")
			.Where((Post p) => p.Id == id)
			.Return((u, p, m) => new
			{
				User = u.As<User>(),
				Post = p.As<Post>(),
				Media = m.As<Media>()
			});
		
		var result = await query.GetResult(cancellationToken);
		return PopulatePostEntity(result.Post, result.User, result.Media);
	}

	public async Task<IEnumerable<Post>> GetByUserId(UserId userId)
	{
		var query = _graphClient
		            .Cypher
		            .Match(
			            $"(u: {nameof(User)})-[: {PostedRelation}]->(p: {Label})-[: {HasMediaRelation}]->(m: {nameof(Media)})")
		            .Where((User u) => u.Id == userId)
		            .With("u, p, m")
		            .OrderByDescending($"p.{CreatedOn}")
		            .With("u, collect(p) as posts, collect(m) as photos")
		            .Return((u, photos, posts) => new
		            {
			            Posts = posts.As<IEnumerable<Post>>(),
			            User = u.As<User>(),
			            Photos = photos.As<IEnumerable<Media>>()
		            });

		var result = await query.GetResult();
		return result.Posts
		             .Zip(result.Photos,
			             (post, media) => PopulatePostEntity(post, result.User, media));
	}

	public async Task<Post?> FindWithDetails(Guid id, CancellationToken cancellationToken = default)
	{
		var query = _graphClient
			.Cypher
			.Match($"(m: {nameof(Media)})<-[: {HasMediaRelation}]-(p: {Label})<-[r: {PostedRelation}]-(u: {nameof(User)})")
			.Where((Post p) => p.Id == id)
			.With("u, p, m")
			.OptionalMatch($"(lu: {nameof(User)})-[l: {LikedRelation}]->(p)")
			.With("u, p, m, collect(lu) as likedBy, collect(l) as likes")
			.OptionalMatch($"(cu: {nameof(User)})-[: {CommentedRelation}]->(c: {nameof(Comment)})--(p)")
			.With("p, u, m, likedBy, likes, collect(cu) as commentedBy, collect(c) as comments")
			.Return((p, u, m, likes, likedBy, commentedBy, comments) => new
			{
				Media = m.As<Media>(),
				Post = p.As<Post>(),
				User = u.As<User>(),
				Likes = likes.As<IEnumerable<Liked>>(),
				LikedBy = likedBy.As<IEnumerable<User>>(),
				CommentedBy = commentedBy.As<IEnumerable<User>>(),
				Comments = comments.As<IEnumerable<Comment>>()
			});

		var result = await query.GetResult(cancellationToken);

		return PopulatePostEntityDetails(
			result.Post,
			result.User,
			result.Media,
			result.Likes,
			result.LikedBy,
			result.Comments,
			result.CommentedBy);
	}

	public override async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
	{
		var query = _graphClient
		            .Cypher
		            .Match($"(p: {Label})-[: {HasMediaRelation}]->(m: {nameof(Media)})")
		            .Where((Post p) => p.Id == id)
		            .DetachDelete("p, m")
		            .Return<Post>("p");

		return await query.GetResult(cancellationToken) != null;
	}

	private Post PopulatePostEntity(Post post, User user, Media? media = null)
	{
		var userAccessor = _accessorProvider.Get<Post, User>(p => p.Author);
		var mediaAccessor = _accessorProvider.Get<Post, Media>(p => p.Photo);
		
		userAccessor[post] = user;
		mediaAccessor[post] = media;
		
		return post;
	}
	
	private Post PopulatePostEntityDetails(
      Post post,
      User user,
      Media media,
      IEnumerable<Liked> likes,
      IEnumerable<User> likedBy,
      IEnumerable<Comment> comments,
      IEnumerable<User> commentedBy)
	{
		var likesAccessor = _accessorProvider.Get((Post p) => p.Likes);
		likesAccessor[post] = likes.ToList();
		
		var commentsAccessor = _accessorProvider.Get((Post p) => p.Comments);
		commentsAccessor[post] = comments.ToList();

		var userAccessor = _accessorProvider.Get((Post p) => p.Author);
		userAccessor[post] = user;
		
		var mediaAccessor = _accessorProvider.Get((Post p) => p.Photo);
		mediaAccessor[post] = media;
      
		var likedByAccessor = _accessorProvider.Get((Liked l) => l.EntityOne);
		var likedByEnumerator = likedBy.GetEnumerator();
      
		foreach (var like in likes)
		{
			likedByEnumerator.MoveNext();
			likedByAccessor[like] = likedByEnumerator.Current;
		}
		
		var authorAccessor = _accessorProvider.Get( (Comment c) => c.Author);
		var commentedByEnumerator = commentedBy.GetEnumerator();
		
		foreach (var comment in comments)
		{
			commentedByEnumerator.MoveNext();
			authorAccessor[comment] = commentedByEnumerator.Current;
		}
		
		return post;
    }
}