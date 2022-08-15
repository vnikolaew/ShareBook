using System.Net;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ShareBook.Infrastructure.Common.Extensions;
using ShareBook.Infrastructure.Images.Settings;

namespace ShareBook.Infrastructure.Images.Services;

public class AzureBlobService : IFileStorageUploadService
{
	private readonly AzureBlobStorageSettings _settings;
	private readonly BlobContainerClient _containerClient;
	private const string DefaultCacheControl = "max-age=3600, public";

	public AzureBlobService(AzureBlobStorageSettings settings)
	{
		_settings = settings;
		_containerClient = new BlobServiceClient(settings.ConnectionString)
			.GetBlobContainerClient(settings.ContainerName);
	}
	
	public async Task<bool> UploadFileAsync(string absoluteFilePath, string blobName)
	{
		await using var fileStream = File.Open(absoluteFilePath, FileMode.Open, FileAccess.Read);
		var blobClient = _containerClient.GetBlobClient(blobName);
		var blobHeaders = new BlobHttpHeaders
		{
			ContentType = blobName.GetContentType(),
			CacheControl = DefaultCacheControl
		};

		var result = await blobClient.UploadAsync(fileStream, blobHeaders);
		var response = result.GetRawResponse();

		return response.Status == (int) HttpStatusCode.Created;
	}

	public async Task<bool> UploadFileAsync(Stream content, string fileName)
	{
		var blobClient = _containerClient.GetBlobClient(fileName);
		var blobHeaders = new BlobHttpHeaders
		{
			ContentType = fileName.GetContentType(),
			CacheControl = DefaultCacheControl
		};

		var result = await blobClient.UploadAsync(content, blobHeaders);
		var response = result.GetRawResponse();

		return response.Status == (int) HttpStatusCode.Created;
	}

	public string GetAbsoluteFileUrl(string fileName)
		=> $"https://{_settings.AccountName}.blob.{_settings.EndpointSuffix}/images/{fileName}";

	public async Task<bool> DeleteFileAsync(string blobName)
	{
		var blobClient = _containerClient.GetBlobClient(blobName);
		
		var result = await blobClient.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots);

		return result.Status == (int) HttpStatusCode.Accepted;
	}
}