using ShareBook.Application.Common.Mappings;
using ShareBook.Domain.Models.Post;

namespace ShareBook.Application.Posts.Common;
public class PostOutputModel : IMapFrom<Post>
{
	public string Id { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime LastModifiedOn { get; set; }

	public string Content { get; set; }

	public PostPhotoOutputModel Media { get; set; }

	public void Mapping(AutoMapper.Profile mapper)
		=> mapper
		   .CreateMap<Post, PostOutputModel>()
		   .ForMember(m => m.Content,
			   cfg => cfg.MapFrom(p => p.Content.Value))
		   .ForMember(m => m.Id,
			   cfg => cfg.MapFrom(p => p.Id.ToString()))
		   .ForMember(m => m.CreatedOn,
			   cfg => cfg.MapFrom(p => p.CreatedOn))
		   .ForMember(m => m.LastModifiedOn,
			   cfg => cfg.MapFrom(p => p.ModifiedOn))
		   .ForMember(m => m.Media,
			   cfg => cfg.MapFrom(p => p.Photo));
}