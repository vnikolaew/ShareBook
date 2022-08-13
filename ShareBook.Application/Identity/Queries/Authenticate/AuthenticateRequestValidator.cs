using FluentValidation;
using static ShareBook.Domain.Models.ModelConstants.User;

namespace ShareBook.Application.Identity.Queries.Authenticate;

public class AuthenticateRequestValidator : AbstractValidator<AuthenticateRequest>
{
	public AuthenticateRequestValidator()
	{
		RuleFor(r => r.Email)
			.MinimumLength(MinEmailLength)
			.MaximumLength(MaxEmailLength);
		
		RuleFor(r => r.Password)
			.MinimumLength(MinPasswordLength)
			.MaximumLength(MaxPasswordLength);
	}
}