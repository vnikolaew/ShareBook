using FluentValidation;
using ShareBook.Application.Comments.Commands.Edit;
using static ShareBook.Domain.Models.ModelConstants.Post;

namespace ShareBook.Application.Comments.Commands.Create;

public class EditCommentValidator : AbstractValidator<EditCommentRequest>
{
	public EditCommentValidator()
		=> RuleFor(r => r.Content)
		   .MinimumLength(MinContentLength)
		   .MaximumLength(MaxContentLength);
}