using ShareBook.Application.Common;
using ShareBook.Application.Common.Contracts;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Follows.Commands.Follow;

public class FollowUserRequest : EntityCommand<Guid>, IRequest<Result>
{
	public class FollowUserHandler : IRequestHandler<FollowUserRequest, Result>
	{
		private readonly IFollowService _follows;
		private readonly ICurrentUser _currentUser;
		private readonly IDateTime _dateTime;

		public FollowUserHandler(
			IFollowService follows,
			ICurrentUser currentUser,
			IDateTime dateTime)
		{
			_follows = follows;
			_currentUser = currentUser;
			_dateTime = dateTime;
		}

		public async Task<Result> Handle(FollowUserRequest request, CancellationToken cancellationToken = default)
		{
			var result = await _follows.Follow(_currentUser.UserId, request.Id, _dateTime.Now);
			return result
				? Result.Success
				: Result.Failure(new[] {"You are already following the requested user. "});
		}
	}
}