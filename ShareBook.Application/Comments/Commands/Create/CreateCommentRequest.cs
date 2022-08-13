using ShareBook.Application.Common;
using ShareBook.Application.Common.Contracts;
using ShareBook.Domain.Common;
using ShareBook.Domain.Models;
using ShareBook.Domain.Models.Post.Repositories;
using ShareBook.Domain.Models.User.Repositories;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Comments.Commands.Create;
public class CreateCommentRequest : EntityCommand<Guid>, IRequest<Result>
{
	public string Content { get; set; }
	
	public class CreateCommentHandler : IRequestHandler<CreateCommentRequest, Result>
	{
		private readonly ICurrentUser _currentUser;
		private readonly IDomainRepository<Comment, Guid> _comments;
		private readonly IUserRepository _userRepository;
		private readonly IPostRepository _postRepository;

		public CreateCommentHandler(
			ICurrentUser currentUser,
			IDomainRepository<Comment, Guid> comments,
			IUserRepository userRepository,
			IPostRepository postRepository)
		{
			_currentUser = currentUser;
			_comments = comments;
			_userRepository = userRepository;
			_postRepository = postRepository;
		}

		public async Task<Result> Handle(CreateCommentRequest request, CancellationToken cancellationToken = default)
		{
			var user = await _userRepository.FindAsync(_currentUser.UserId, cancellationToken);
			var post = await _postRepository.FindAsync(request.Id, cancellationToken);
			if (post is null)
			{
				return Result.Failure(new []{ "Requested post does not exist."});
			}
			
			var comment = await _comments.SaveAsync(new Comment(user, request.Content, post), cancellationToken);
			return comment is { }
				? Result.Success
				: Result.Failure(new[] {"Could not save the requested comment."});
		}
	}

}