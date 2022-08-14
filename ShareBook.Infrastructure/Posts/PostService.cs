using ShareBook.Application.Posts;
using ShareBook.Application.Posts.Commands.Create;
using ShareBook.Domain.Models.Media;
using ShareBook.Domain.Models.Post;
using ShareBook.Domain.Models.Post.Factories;
using ShareBook.Domain.Models.Post.Repositories;
using ShareBook.Domain.Models.User;
using ShareBook.Infrastructure.Common.Extensions;
using ShareBook.Infrastructure.Images.Services;

namespace ShareBook.Infrastructure.Posts;

public class PostService : IPostService
{
	private readonly IPostRepository _postRepository;
	private readonly IFileStorageUploadService _uploadService;
	private readonly IPostFactory _postFactory;

	public PostService(
		IPostRepository postRepository,
		IFileStorageUploadService uploadService,
		IPostFactory postFactory)
	{
		_postRepository = postRepository;
		_uploadService = uploadService;
		_postFactory = postFactory;
	}

	public Task<Post?> Find(Guid id)
		=> _postRepository.FindAsync(id);

	public Task<Post?> Details(Guid id)
		=> _postRepository.FindWithDetails(id);

	public async Task<Post?> Create(User user, CreatePostRequest request)
	{
		string mediaFileName = $"Original_{request.Id}.{request.Media.MediaName.GetFileExtension()}";

		var photo = new Media(mediaFileName, request.Media.ContentType, _uploadService.GetAbsoluteFileUrl(mediaFileName));
		var post = _postFactory
		           .WithAuthor(user)
		           .WithContent(request.Content)
		           .WithPhoto(photo)
		           .Build();

		Task<bool> imageUploadTask = _uploadService.UploadFileAsync(request.Media.Content, mediaFileName);
		Task<Post> imageSaveTask = _postRepository.SaveAsync(post);

		await Task.WhenAll(imageSaveTask, imageUploadTask);
		return imageSaveTask.Result is Post newPost && imageUploadTask.Result
			? newPost
			: default;
	}

	public Task<IEnumerable<Post>> GetAllByUserId(UserId userId)
		=> _postRepository.GetByUserId(userId);

	public Task<bool> Delete(Guid postId)
		=> _postRepository.DeleteAsync(postId);

	public Task<Post> Update(Post newPost)
		=> _postRepository.UpdateAsync(newPost);
}