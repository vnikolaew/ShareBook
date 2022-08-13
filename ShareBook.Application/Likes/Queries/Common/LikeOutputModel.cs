using ShareBook.Application.Common.Mappings;
using ShareBook.Domain.Relationships;

namespace ShareBook.Application.Likes.Queries.Common;
public class LikeOutputModel : IMapFrom<Liked>
{
	public string Id { get; set; }
	public string UserId { get; set; }
	public string Username { get; set; }
	public DateTime At { get; set; }

	public void Mapping(AutoMapper.Profile profile)
		=> profile.CreateMap<Liked, LikeOutputModel>()
		          .ForMember(l => l.Id,
			          cfg => cfg.MapFrom(l => l.Id.ToString()))
		          .ForMember(l => l.UserId,
			          cfg => cfg.MapFrom(l => l.EntityOne.Id.Value))
		          .ForMember(l => l.Username,
			          cfg => cfg.MapFrom(l => l.EntityOne.Username.Value))
		          .ForMember(l => l.At,
			          cfg => cfg.MapFrom(l => l.CreatedOn));
}