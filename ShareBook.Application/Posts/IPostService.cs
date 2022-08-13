using ShareBook.Application.Posts.Commands.Create;
using ShareBook.Domain.Models.Post;
using ShareBook.Domain.Models.User;

namespace ShareBook.Application.Posts;

public interface IPostService
{
	Task<Post?> Find(Guid id);
	Task<Post?> Details(Guid id);
	Task<Post?> Create(User user, CreatePostRequest request);
	Task<IEnumerable<Post>> GetAllByUserId(UserId userId);
	Task<bool> Delete(Guid postId);
	Task<Post> Update(Post newPost);
}