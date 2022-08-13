using ShareBook.Application.Common.Mappings;
using ShareBook.Domain.Models;

namespace ShareBook.Application.Posts.Common;
public class CommentOutputModel : IMapFrom<Comment>
{
	public string Id { get; set; }
	public string Author { get; set; }
	public DateTime CreatedOn { get; set; }
	public string Content { get; set; }

	public void Mapping(AutoMapper.Profile profile)
		=> profile
		   .CreateMap<Comment, CommentOutputModel>()
		   .ForMember(c => c.Id,
			   cfg => cfg.MapFrom(c => c.Id.ToString()))
		   .ForMember(c => c.Author,
			   cfg => cfg.MapFrom(c => c.Author.Username))
		   .ForMember(c => c.CreatedOn,
			   cfg => cfg.MapFrom(c => c.CreatedOn))
		   .ForMember(c => c.Content,
			   cfg => cfg.MapFrom(c => c.Content.Value));
}