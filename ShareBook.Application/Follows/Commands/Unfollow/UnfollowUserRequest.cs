using ShareBook.Application.Common;
using ShareBook.Application.Common.Contracts;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Follows.Commands.Follow;
public class UnfollowUserRequest : EntityCommand<Guid>, IRequest<Result>
{
	public class UnfollowUserHandler : IRequestHandler<UnfollowUserRequest, Result>
	{
		private readonly IFollowService _follows;
		private readonly ICurrentUser _currentUser;

		public UnfollowUserHandler(
			IFollowService follows,
			ICurrentUser currentUser)
		{
			_follows = follows;
			_currentUser = currentUser;
		}

		public async Task<Result> Handle(UnfollowUserRequest request, CancellationToken cancellationToken = default)
		{
			var result = await _follows.Unfollow(_currentUser.UserId, request.Id);
			return result
				? Result.Success
				: Result.Failure(new[] {"You are not following the requested user. "});
		}
	}
}