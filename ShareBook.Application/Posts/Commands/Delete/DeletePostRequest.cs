using ShareBook.Application.Common;
using ShareBook.Application.Common.Contracts;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Posts.Commands.Delete;
public class DeletePostRequest : EntityCommand<Guid>, IRequest<Result>
{
	public class DeletePostHandler : IRequestHandler<DeletePostRequest, Result>
	{
		private readonly ICurrentUser _currentUser;
		private readonly IPostService _postService;

		public DeletePostHandler(
			ICurrentUser currentUser,
			IPostService postService)
		{
			_currentUser = currentUser;
			_postService = postService;
		}

		public async Task<Result> Handle(DeletePostRequest request, CancellationToken cancellationToken = default)
		{
			var post = await _postService.Find(request.Id);

			if (post is null || post?.Author.Id != _currentUser.UserId)
			{
				return Result.Failure(new []{ "Post does not exist or current user is not an owner of the post. "});
			}

			return await _postService.Delete(request.Id);
		}
	}

}