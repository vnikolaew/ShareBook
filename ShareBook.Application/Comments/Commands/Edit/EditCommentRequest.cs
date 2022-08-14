using ShareBook.Application.Common;
using ShareBook.Application.Common.Contracts;
using ShareBook.Domain.Common;
using ShareBook.Domain.Models;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Comments.Commands.Edit;
public class EditCommentRequest : EntityCommand<Guid>, IRequest<Result>
{
	public string Content { get; set; }

	public class EditCommentHandler : IRequestHandler<EditCommentRequest, Result>
	{
		private readonly ICurrentUser _currentUser;
		private readonly IDomainRepository<Comment, Guid> _comments;

		public EditCommentHandler(
			ICurrentUser currentUser,
			IDomainRepository<Comment, Guid> comments)
		{
			_currentUser = currentUser;
			_comments = comments;
		}

		public async Task<Result> Handle(EditCommentRequest request, CancellationToken cancellationToken = default)
		{
			var comment = await _comments.FindAsync(request.Id, cancellationToken);
			if (comment is null)
			{
				return "Requested comment does not exist. ";
			}
			
			if (comment.Author.Id != _currentUser.UserId)
			{
				return "You do not own the requested comment.";
			}
			
			comment.EditContent(request.Content);
			var result = await _comments.SaveAsync(comment, cancellationToken);

			return result is null
				? "Could not save the requested comment."
					: Result.Success;
		}
	}
}