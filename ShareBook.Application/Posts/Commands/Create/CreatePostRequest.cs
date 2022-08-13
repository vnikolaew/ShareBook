using ShareBook.Application.Common;
using ShareBook.Application.Common.Contracts;
using ShareBook.Application.Identity;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Posts.Commands.Create;
public class CreatePostRequest : EntityCommand<Guid>, IRequest<Result>
{
	public string Content { get; set; }
	public PostMediaModel Media { get; set; }

	public class CreatePostHandler : IRequestHandler<CreatePostRequest, Result>
	{
		private readonly IPostService _postService;
		private readonly IIdentityService _identity;
		private readonly ICurrentUser _currentUser;

		public CreatePostHandler(
			IPostService postService,
			ICurrentUser currentUser,
			IIdentityService identity)
		{
			_postService = postService;
			_currentUser = currentUser;
			_identity = identity;
		}

		public async Task<Result> Handle(CreatePostRequest request, CancellationToken cancellationToken = default)
		{
			var user = await _identity.FindById(_currentUser.UserId, cancellationToken);
			
			var newPost = await _postService.Create(user, request.WithId(Guid.NewGuid()));

			return newPost is not null;
		}
	}
}