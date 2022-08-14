using ShareBook.Application.Common;
using ShareBook.Application.Common.Contracts;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Posts.Commands.Edit;
public class EditPostRequest : EntityCommand<Guid>, IRequest<Result>
{
	public string Content { get; set; }

	public class EditPostHandler : IRequestHandler<EditPostRequest, Result>
	{
		private readonly IPostService _postService;
		private readonly ICurrentUser _currentUser;

		public EditPostHandler(
			IPostService postService,
			ICurrentUser currentUser)
		{
			_postService = postService;
			_currentUser = currentUser;
		}

		public async Task<Result> Handle(EditPostRequest request, CancellationToken cancellationToken = default)
		{
			var post = await _postService.Find(request.Id);

			if (post is null || post.Author.Id != _currentUser.UserId)
			{
				return "Post does not exist or current user is not an owner of the post. ";
			}
			
			post.EditContent(request.Content);
			await _postService.Update(post);
			
			return Result.Success;
		}
	}
}