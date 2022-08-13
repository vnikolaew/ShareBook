using ShareBook.Application.Common.Mappings;
using DomainProfile = ShareBook.Domain.Models.Profile.Profile;

namespace ShareBook.Application.Profile.Queries.Mine;
public class ProfileOutputModel : IMapFrom<DomainProfile>
{
	public string Id { get; set; }
	public string Bio { get; set; }
	public string FullName { get; set; }
	public string ProfilePhotoUrl { get; set; }
	public string Gender { get; set; }
	
	public void Mapping(AutoMapper.Profile profile)
		=> profile.CreateMap<DomainProfile, ProfileOutputModel>()
		          .ForMember(m => m.Id,
			          cfg => cfg.MapFrom(p => p.Id.ToString()))
		          .ForMember(m => m.Bio,
			          cfg => cfg.MapFrom(p => p.Bio.Value))
		          .ForMember(m => m.FullName,
			          cfg => cfg.MapFrom(p => p.FullName.Value))
		          .ForMember(m => m.ProfilePhotoUrl,
			          cfg => cfg.MapFrom(p => p.Photo.AbsoluteUrl))
		          .ForMember(m => m.Gender,
			          cfg => cfg.MapFrom(p => p.Gender));
}