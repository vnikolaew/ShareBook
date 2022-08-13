using ShareBook.Domain.Models.User;
using ShareBook.Domain.Relationships;

namespace ShareBook.Application.Likes;

public interface ILikesService
{
	Task<bool> Like(UserId likerId, Guid likedPostId,
		DateTime createdOn, CancellationToken cancellationToken = default);

	Task<bool> Unlike(UserId likerId, Guid likedPostId,
		CancellationToken cancellationToken = default);

	Task<IEnumerable<Liked>> GetAllByPostId(Guid id, CancellationToken cancellationToken = default);
}