using ShareBook.Application.Profile.Commands.EditPhoto;
using ShareBook.Domain.Models.User;

namespace ShareBook.Application.Profile;

public interface IMediaService
{
	Task<bool> SaveProfileMediaAsync(UserId userId, EditProfilePhotoRequest request);
}