using ShareBook.Domain.Common;
using ShareBook.Domain.Models.Profile;

namespace ShareBook.Domain.Models.User;
public class User : Entity<UserId>, IAggregateRoot
{
  private readonly ICollection<ShareBook.Domain.Models.Post.Post> _posts = (ICollection<ShareBook.Domain.Models.Post.Post>) new List<ShareBook.Domain.Models.Post.Post>();

  public Username Username { get; private set; }

  public Email Email { get; private set; }

  public Password Password { get; private set; }

  public ShareBook.Domain.Models.Profile.Profile Profile { get; private set; }

  public User(
    Username username,
    Email email,
    Password password,
    ShareBook.Domain.Models.Profile.Profile profile)
  {
    Username = username;
    Email = email;
    Password = password;
    Profile = profile;
  }

  public User() { }

  public IReadOnlyCollection<ShareBook.Domain.Models.Post.Post> Posts => (IReadOnlyCollection<ShareBook.Domain.Models.Post.Post>) this._posts.ToList<ShareBook.Domain.Models.Post.Post>().AsReadOnly();

  public void PublishPost(ShareBook.Domain.Models.Post.Post post)
    => this._posts.Add(post);

  public ShareBook.Domain.Models.User.User UpdateBio(Bio bio)
  {
    Profile.UpdateBio(bio);
    return this;
  }

  public ShareBook.Domain.Models.User.User UpdateGender(Gender gender)
  {
    Profile.UpdateGender(gender);
    return this;
  }
}