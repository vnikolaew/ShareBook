using ShareBook.Application.Posts.Commands.Create;
using ShareBook.Application.Profile.Commands.EditPhoto;

namespace ShareBook.Web.Extensions;

public static class RequestExtensions
{
	public static EditProfilePhotoRequest ToRequest(this IFormFile file)
		=> new()
		{
			Content = file.OpenReadStream(),
			ContentType = file.ContentType,
			FileName = file.FileName
		};

	public static PostMediaModel ToPostMediaModel(this IFormFile file)
		=> new()
		{
			Content = file.OpenReadStream(),
			ContentType = file.ContentType,
			MediaName = file.FileName
		};
}	