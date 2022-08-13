using ShareBook.Domain.Models.User;

namespace ShareBook.Application.Follows;

public interface IFollowService
{
	Task<bool> Follow(UserId followerId, UserId followeeId, DateTime dateTime);
	Task<bool> Unfollow(UserId followerId, UserId followeeId);
	Task<IEnumerable<Domain.Relationships.Follows>> GetFollowers(UserId followeeId);
	Task<IEnumerable<ShareBook.Domain.Relationships.Follows>> GetFollowing(UserId followerId);
}