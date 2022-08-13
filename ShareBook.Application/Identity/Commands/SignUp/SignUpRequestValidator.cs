using FluentValidation;
using static ShareBook.Domain.Models.ModelConstants.User;

namespace ShareBook.Application.Identity.Commands.SignUp;

public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
	public SignUpRequestValidator()
	{
		RuleFor(r => r.Email)
			.MinimumLength(MinEmailLength)
			.MaximumLength(MaxEmailLength);

		RuleFor(r => r.Username)
			.MinimumLength(MinUsernameLength)
			.MaximumLength(MaxUsernameLength);
		
		RuleFor(r => r.Password)
			.MinimumLength(MinPasswordLength)
			.MaximumLength(MaxPasswordLength);
	}
	
}