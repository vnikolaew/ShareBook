using ShareBook.Application.Common;
using ShareBook.Application.Common.Contracts;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Likes.Commands.Like;
public class LikePostRequest : EntityCommand<Guid>, IRequest<Result>
{
	public class LikePostHandler : IRequestHandler<LikePostRequest, Result>
	{
		private readonly ILikesService _likes;
		private readonly ICurrentUser _currentUser;
		private readonly IDateTime _dateTime;

		public LikePostHandler(
			ILikesService likes,
			ICurrentUser currentUser,
			IDateTime dateTime)
		{
			_likes = likes;
			_currentUser = currentUser;
			_dateTime = dateTime;
		}

		public async Task<Result> Handle(LikePostRequest request, CancellationToken cancellationToken = default)
		{
			var result = await _likes.Like(_currentUser.UserId, request.Id, _dateTime.Now, cancellationToken);
			return result
				? Result.Success
				: Result.Failure(new[] {"You've already liked that post before. "});
		}
	}

}