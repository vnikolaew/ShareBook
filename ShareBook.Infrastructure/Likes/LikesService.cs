using ShareBook.Application.Likes;
using ShareBook.Domain.Models.User;
using ShareBook.Domain.Relationships;
using ShareBook.Domain.Relationships.Repositories;

namespace ShareBook.Infrastructure.Likes;

public class LikesService : ILikesService
{
	private readonly ILikeRepository _likes;

	public LikesService(ILikeRepository likes)
		=> _likes = likes;

	public async Task<bool> Like(UserId likerId, Guid likedPostId, DateTime createdOn, CancellationToken cancellationToken = default)
	{
		var likeExists = await _likes.ExistsAsync(likerId, likedPostId, cancellationToken);
		if (likeExists)
		{
			return false;
		}

		await _likes.CreateAsync(likerId, likedPostId, new Liked(createdOn), cancellationToken);
		return true;
	}

	public async Task<bool> Unlike(UserId likerId, Guid likedPostId, CancellationToken cancellationToken = default)
	{
		var likeExists = await _likes.ExistsAsync(likerId, likedPostId, cancellationToken);
		if (!likeExists)
		{
			return false;
		}

		await _likes.DeleteAsync(likerId, likedPostId, cancellationToken);
		return true;
	}

	public Task<IEnumerable<Liked>> GetAllByPostId(Guid id, CancellationToken cancellationToken = default)
		=> _likes.GetAllByPostId(id, cancellationToken);
}