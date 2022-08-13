using ShareBook.Application.Common;
using ShareBook.Application.Common.Contracts;
using ShareBook.Domain.Common;
using ShareBook.Domain.Models.Profile;
using ShareBook.Domain.Models.User.Repositories;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Profile.Commands.Edit;
public class EditProfileRequest : IRequest<Result>
{
	public string Bio { get; set; }
	public Gender Gender { get; set; }

	public class EditProfileHandler : IRequestHandler<EditProfileRequest, Result>
	{
		private readonly ICurrentUser _currentUser;
		private readonly IUserRepository _userRepository;
		private readonly IDomainRepository<Domain.Models.Profile.Profile, Guid> _profiles;

		public EditProfileHandler(
			ICurrentUser currentUser,
			IUserRepository userRepository,
			IDomainRepository<Domain.Models.Profile.Profile, Guid> profiles)
		{
			_currentUser = currentUser;
			_userRepository = userRepository;
			_profiles = profiles;
		}

		public async Task<Result> Handle(EditProfileRequest request, CancellationToken cancellationToken = default)
		{
			var user = await _userRepository.FindAsync(_currentUser.UserId, cancellationToken);

			user.UpdateBio(request.Bio)
			    .UpdateGender(request.Gender);

			var profile = await _profiles.UpdateAsync(user.Profile, cancellationToken);
			return profile is not null;
		}
	}
}