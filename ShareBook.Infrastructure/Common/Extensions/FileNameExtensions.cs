using Microsoft.AspNetCore.StaticFiles;

namespace ShareBook.Infrastructure.Common.Extensions;

public static class FileNameExtensions
{
	private static readonly FileExtensionContentTypeProvider ContentTypeProvider
		= new();
	private const string DefaultContentType = "application/octet-stream";

	public static string GetContentType(this string fileName)
	{
		return !ContentTypeProvider.TryGetContentType(fileName, out var contentType)
			? DefaultContentType
			: contentType;
	}

	public static string GetFileExtension(this string fileName)
		=> fileName[(fileName.LastIndexOf('.') + 1)..];
}