using ShareBook.Application.Follows;
using ShareBook.Domain.Models.User;
using ShareBook.Domain.Relationships.Repositories;
using DomainFollows = ShareBook.Domain.Relationships.Follows;

namespace ShareBook.Infrastructure.Follows;

public class FollowsService : IFollowService
{
	private readonly IFollowRepository _follows;

	public FollowsService(IFollowRepository follows)
		=> _follows = follows;

	public async Task<bool> Follow(UserId followerId, UserId followeeId, DateTime dateTime)
	{
		var exists = await _follows.ExistsAsync(followerId, followeeId);
		if (exists)
		{
			return false;
		}

		await _follows.CreateAsync(
			followerId,
			followeeId,
			new DomainFollows(dateTime));
		
		return true;
	}

	public async Task<bool> Unfollow(UserId followerId, UserId followeeId)
	{
		var followExists = await _follows.ExistsAsync(followerId, followeeId);
		if (!followExists)
		{
			return false;
		}

		return await _follows.DeleteAsync(followerId, followeeId);
	}

	public Task<IEnumerable<DomainFollows>> GetFollowers(UserId followeeId)
		=> _follows.GetFollowersAsync(followeeId);

	public Task<IEnumerable<DomainFollows>> GetFollowing(UserId followerId)
		=> _follows.GetFollowsAsync(followerId);
}