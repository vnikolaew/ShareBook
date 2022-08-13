using ShareBook.Domain.Models.Post;
using LikeOutputModel = ShareBook.Application.Likes.Queries.Common.LikeOutputModel;

namespace ShareBook.Application.Posts.Common;
public class PostDetailsOutputModel : PostOutputModel
{
	public string Author { get; set; }
	public IEnumerable<LikeOutputModel> Likes { get; set; }
	public IEnumerable<CommentOutputModel> Comments { get; set; }

	public new void Mapping(AutoMapper.Profile profile)
		=> profile
				  .CreateMap<Post, PostDetailsOutputModel>()
		          .IncludeBase<Post, PostOutputModel>()
		          .ForMember(m => m.Author,
			          cfg => cfg.MapFrom(p => p.Author.Username.Value))
		          .ForMember(m => m.Likes,
			          cfg => cfg.MapFrom(p => p.Likes))
		          .ForMember(m => m.Comments,
			          cfg => cfg.MapFrom(p => p.Comments));
}