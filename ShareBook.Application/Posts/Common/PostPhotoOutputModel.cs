using AutoMapper;
using ShareBook.Application.Common.Mappings;
using ShareBook.Domain.Models.Media;

namespace ShareBook.Application.Posts.Common;

public class PostPhotoOutputModel : IMapFrom<Media>
{
	public string Id { get; set; }
	public string PhotoUrl { get; set; }

	public void Mapping(AutoMapper.Profile profile)
		=> profile.CreateMap<Media, PostPhotoOutputModel>()
		          .ForMember(m => m.Id,
			          cfg => cfg.MapFrom(m => m.Id.ToString()))
		          .ForMember(m => m.PhotoUrl,
			          cfg => cfg.MapFrom(m => m.AbsoluteUrl));
}