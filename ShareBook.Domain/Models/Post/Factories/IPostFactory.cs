using ShareBook.Domain.Common;

namespace ShareBook.Domain.Models.Post.Factories;
public interface IPostFactory : IFactory<Post>
{
  IPostFactory WithAuthor(User.User user);

  IPostFactory WithContent(Content content);

  IPostFactory WithContent(string content);

  IPostFactory WithPhoto(Media.Media photo);
}