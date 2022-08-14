namespace ShareBook.Infrastructure.Images.Services;

public interface IFileStorageUploadService
{
	Task<bool> UploadFileAsync(string absoluteFilePath, string fileName);

	Task<bool> UploadFileAsync(Stream data, string fileName);

	string GetAbsoluteFileUrl(string fileName);

	Task<bool> DeleteFileAsync(string blobName);	
}