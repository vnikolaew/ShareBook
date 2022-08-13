using FluentValidation;
using static ShareBook.Domain.Models.ModelConstants.Post;

namespace ShareBook.Application.Comments.Commands.Create;

public class CreateCommentValidator : AbstractValidator<CreateCommentRequest>
{
	public CreateCommentValidator()
		=> RuleFor(r => r.Content)
		   .MinimumLength(MinContentLength)
		   .MaximumLength(MaxContentLength);
}