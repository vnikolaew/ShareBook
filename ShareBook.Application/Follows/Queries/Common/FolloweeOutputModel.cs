using ShareBook.Application.Common.Mappings;
using ShareBook.Application.Posts.Common;
using DomainFollows = ShareBook.Domain.Relationships.Follows;

namespace ShareBook.Application.Follows.Queries.Common;
public class FolloweeOutputModel : IMapFrom<DomainFollows>
{
	private readonly ICollection<PostOutputModel> Posts
		= new List<PostOutputModel>();
	public string Id { get; set; }
	public string Username { get; private set; }
	public string Email { get; private set; }
	public DateTime Since { get; set; }
	public string Gender { get; set; }
	

	public void Mapping(AutoMapper.Profile profile)
		=> profile.CreateMap<DomainFollows, FolloweeOutputModel>()
		          .ForMember(m => m.Id,
			          cfg => cfg.MapFrom(f => f.Id))
		          .ForMember(m => m.Username,
			          cfg => cfg.MapFrom(f => f.EntityTwo.Username.Value))
		          .ForMember(m => m.Email,
			          cfg => cfg.MapFrom(f => f.EntityTwo.Email.Value))
		          .ForMember(m => m.Since,
			          cfg => cfg.MapFrom(f => f.Since))
		          .ForMember(m => m.Gender,
			          cfg => cfg.MapFrom(f => f.EntityTwo.Profile.Gender));
}