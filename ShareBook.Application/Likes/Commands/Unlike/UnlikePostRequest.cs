using ShareBook.Application.Common;
using ShareBook.Application.Common.Contracts;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Likes.Commands.Like;

public class UnlikePostRequest : EntityCommand<Guid>, IRequest<Result>
{
	public class UnlikePostHandler : IRequestHandler<UnlikePostRequest, Result>
	{
		private readonly ILikesService _likes;
		private readonly ICurrentUser _currentUser;

		public UnlikePostHandler(
			ILikesService likes,
			ICurrentUser currentUser)
		{
			_likes = likes;
			_currentUser = currentUser;
		}

		public async Task<Result> Handle(UnlikePostRequest request, CancellationToken cancellationToken = default)
		{
			var result = await _likes.Unlike(_currentUser.UserId, request.Id, cancellationToken);
			return result
				? Result.Success
				: Result.Failure(new[] {"You haven't liked that post before. "});
		}
	}

}