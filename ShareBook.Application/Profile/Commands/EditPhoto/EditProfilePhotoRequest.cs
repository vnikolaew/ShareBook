using ShareBook.Application.Common;
using ShareBook.Application.Common.Contracts;
using ShareBook.Shared.Abstractions.Requests;

namespace ShareBook.Application.Profile.Commands.EditPhoto;
public class EditProfilePhotoRequest : EntityCommand<Guid>, IRequest<Result>
{
	public Stream Content { get; set; }
	public string FileName { get; set; }
	public string ContentType { get; set; }

	public class EditProfilePhotoHandler : IRequestHandler<EditProfilePhotoRequest, Result>
	{
		private readonly IMediaService _mediaService;
		private readonly ICurrentUser _currentUser;

		public EditProfilePhotoHandler(
			IMediaService mediaService,
			ICurrentUser currentUser)
		{
			_mediaService = mediaService;
			_currentUser = currentUser;
		}

		public async Task<Result> Handle(EditProfilePhotoRequest request, CancellationToken cancellationToken = default)
		{
			var result = await _mediaService
				.SaveProfileMediaAsync(_currentUser.UserId, request.WithId(Guid.NewGuid()));

			return result
				? "Photo upload was unsuccessful."
				: Result.Success;
		}
	}
}