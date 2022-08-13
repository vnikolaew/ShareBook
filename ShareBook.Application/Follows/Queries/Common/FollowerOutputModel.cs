using ShareBook.Application.Common.Mappings;
using ShareBook.Application.Posts.Common;

namespace ShareBook.Application.Follows.Queries.Common;
using DomainFollows = ShareBook.Domain.Relationships.Follows;
public class FollowerOutputModel : IMapFrom<DomainFollows>
{
	private readonly ICollection<PostOutputModel> Posts
		= new List<PostOutputModel>();
	public string Id { get; set; }
	public string Username { get; private set; }
	public string Email { get; private set; }
	public DateTime Since { get; set; }

	public string Gender { get; set; }
	public void Mapping(AutoMapper.Profile profile)
    		=> profile.CreateMap<DomainFollows, FollowerOutputModel>()
    		          .ForMember(m => m.Id,
    			          cfg => cfg.MapFrom(f => f.Id))
    		          .ForMember(m => m.Username,
    			          cfg => cfg.MapFrom(f => f.EntityOne.Username.Value))
    		          .ForMember(m => m.Email,
    			          cfg => cfg.MapFrom(f => f.EntityOne.Email.Value))
    		          .ForMember(m => m.Since,
    			          cfg => cfg.MapFrom(f => f.Since))
    		          .ForMember(m => m.Gender,
    			          cfg => cfg.MapFrom(f => f.EntityOne.Profile.Gender));
}