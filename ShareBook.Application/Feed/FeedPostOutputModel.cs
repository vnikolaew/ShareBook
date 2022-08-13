using ShareBook.Application.Common.Mappings;
using ShareBook.Application.Posts.Common;
using ShareBook.Domain.Models.Post;
using LikeOutputModel = ShareBook.Application.Likes.Queries.Common.LikeOutputModel;

namespace ShareBook.Application.Feed;
public class FeedPostOutputModel : IMapFrom<Post>
{
	public string Id { get; set; }

	public string Author { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime ModifiedOn { get; set; }

	public string Content { get; set; }

	public PostPhotoOutputModel Media { get; set; }

	public IEnumerable<LikeOutputModel> Likes { get; set; }

	public IEnumerable<CommentOutputModel> Comments { get; set; }

	public void Mapping(AutoMapper.Profile profile)
		=> profile.CreateMap<Post, FeedPostOutputModel>()
		          .ForMember(p => p.Id,
			          cfg => cfg.MapFrom(p => p.Id.ToString()))
		          .ForMember(p => p.Author,
			          cfg => cfg.MapFrom(p => p.Author.Username.Value))
		          .ForMember(p => p.Content,
			          cfg => cfg.MapFrom(p => p.Content.Value))
		          .ForMember(p => p.Content,
			          cfg => cfg.MapFrom(p => p.Content.Value))
		          .ForMember(p => p.Media,
			          cfg => cfg.MapFrom(p => p.Photo))
		          .ForMember(p => p.Likes,
			          cfg => cfg.MapFrom(p => p.Likes))
		          .ForMember(p => p.Comments,
			          cfg => cfg.MapFrom(p => p.Comments));

}