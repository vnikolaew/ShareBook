using FluentValidation;
using static ShareBook.Domain.Models.ModelConstants.Post;

namespace ShareBook.Application.Posts.Commands.Edit;

public class EditPostValidator : AbstractValidator<EditPostRequest>
{
	public EditPostValidator()
		=> RuleFor(r => r.Content)
		   .MinimumLength(MinContentLength)
		   .MaximumLength(MaxContentLength);
}