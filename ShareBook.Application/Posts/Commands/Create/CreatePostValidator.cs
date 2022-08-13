using FluentValidation;
using static ShareBook.Domain.Models.ModelConstants.Post;

namespace ShareBook.Application.Posts.Commands.Create;

public class CreatePostValidator : AbstractValidator<CreatePostRequest>
{
	public CreatePostValidator()
		=> RuleFor(r => r.Content)
		   .MinimumLength(MinContentLength)
		   .MaximumLength(MaxContentLength);
}