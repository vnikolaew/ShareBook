using ShareBook.Application.Profile;
using ShareBook.Application.Profile.Commands.EditPhoto;
using ShareBook.Domain.Models.Media;
using ShareBook.Domain.Models.User;
using ShareBook.Infrastructure.Common.Extensions;

namespace ShareBook.Infrastructure.Images.Services;

public class MediaService : IMediaService
{
	private readonly IFileStorageUploadService _uploadService;
	private readonly IMediaRepository _media;

	public MediaService(
		IFileStorageUploadService uploadService,
		IMediaRepository media)
	{
		_uploadService = uploadService;
		_media = media;
	}

	public async Task<bool> SaveProfileMediaAsync(UserId userId, EditProfilePhotoRequest request)
	{
		var mediaFileName = $"Original_{request.Id}.{request.FileName.GetFileExtension()}";

		var media = new Media(
				mediaFileName,
				request.ContentType,
				_uploadService.GetAbsoluteFileUrl(mediaFileName));
			
		try
		{
			Media photoByUser = await GetPhotoByUser(userId);
			
			var deleteOldPhotoTask =
				photoByUser is not null
					? _uploadService.DeleteFileAsync(photoByUser.MediaName)
					: default;
			var uploadMediaTask = _uploadService.UploadFileAsync(request.Content, mediaFileName);
			var mediaSaveTask = _media.SaveProfileMediaAsync(userId, media);

			await Task.WhenAll(deleteOldPhotoTask, uploadMediaTask, mediaSaveTask);
			
			return uploadMediaTask.Result
			       && mediaSaveTask.Result is not null
			        && deleteOldPhotoTask.Result;

		}
		catch (Exception e)
		{
			return false;
		}
	}

	private Task<Media?> GetPhotoByUser(UserId userId)
		=> _media.GetByUserIdAsync(userId);
}