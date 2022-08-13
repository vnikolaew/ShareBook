using FluentValidation;
using static ShareBook.Domain.Models.ModelConstants.Profile;

namespace ShareBook.Application.Profile.Commands.Edit;

public class EditProfileValidator : AbstractValidator<EditProfileRequest>
{
	public EditProfileValidator()
		=> RuleFor(r => r.Bio)
		   .MinimumLength(MinBioLength)
		   .MaximumLength(MaxBioLength);
}