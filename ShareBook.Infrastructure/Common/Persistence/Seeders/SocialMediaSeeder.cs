using Bogus;
using ShareBook.Application.Common.Contracts;
using ShareBook.Domain.Common;
using ShareBook.Domain.Models;
using ShareBook.Domain.Models.Media;
using ShareBook.Domain.Models.Post;
using ShareBook.Domain.Models.Post.Repositories;
using ShareBook.Domain.Models.Profile;
using ShareBook.Domain.Models.User;
using ShareBook.Domain.Models.User.Repositories;
using ShareBook.Domain.Relationships;
using ShareBook.Domain.Relationships.Repositories;
using ShareBook.Infrastructure.Common.Security;

namespace ShareBook.Infrastructure.Common.Persistence.Seeders;

public class SocialMediaSeeder : IInitialDataSeeder
{
    private readonly IUserRepository _users;
    private readonly IPostRepository _posts;
    private readonly IFollowRepository _follows;
    private readonly ILikeRepository _likes;
    private readonly IMediaRepository _media;
    private readonly IDomainRepository<Comment, Guid> _comments;
    private readonly IDateTime _dateTime;
    public readonly IPasswordHasher _hasher;
    
    private const int UsersCount = 30;
    private const int PostsCount = 300;
    private const int LikesCount = 300;
    private const int FollowsCount = 200;
    private const int CommentsCount = 500;

    public SocialMediaSeeder(
	    IUserRepository users,
	    IPostRepository posts,
	    IFollowRepository follows,
	    ILikeRepository likes,
	    IMediaRepository media,
	    IDomainRepository<Comment, Guid> comments,
	    IDateTime dateTime,
	    IPasswordHasher hasher)
    {
	    _users = users;
	    _posts = posts;
	    _follows = follows;
	    _likes = likes;
	    _media = media;
	    _comments = comments;
	    _dateTime = dateTime;
	    _hasher = hasher;
    }

    public async Task SeedAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
	    var profileFaker = new Faker<Profile>()
	                       .RuleFor(p => p.FullName, f => new FullName(f.Person.FullName))
	                       .RuleFor(p => p.Gender, f => f.PickRandom<Gender>())
	                       .RuleFor(p => p.Bio,
		                       (f, p) => new Bio(
			                       $"Hi, my name is {p.FullName}, I'm {f.Random.Int(15, 60)} years old and I work in {f.Person.Company.Name}."))
	                       .RuleFor(p => p.Photo,
		                       f => new Media($"Original_{Guid.NewGuid()}.jpg", "image/jpeg", f.Internet.Avatar()));

	    var userFaker = new Faker<User>()
	                    .RuleFor(u => u.Profile, _ => profileFaker)
	                    .RuleFor(u => u.Username,
		                    (f, u) => new Username(f.Internet.UserName(u.Profile.FullName.Value.Split(" ")[0])))
	                    .RuleFor(u => u.Email, (f, u) => new Email(f.Internet.Email(u.Username)))
	                    .RuleFor(u => u.Password, (_, u) => new Password(_hasher.Secure($"{u.Username}123")));

	    var users = await CreateUsers(userFaker);
	    await CreateFollows(users);

	    var postFaker = CreatePostFaker(users);
	    var posts = await CreatePosts(postFaker);

	    var commentsFaker = CreateCommentFaker(users, posts);

	    await CreateLikes(users, posts);
	    await CreateComments(commentsFaker);
    }
    
    private Faker<Comment> CreateCommentFaker(
	    IList<User> users,
	    IList<Post> posts)
	    => new Faker<Comment>()
		    .CustomInstantiator(f =>
		     new Comment(
			     users[Randomizer.Seed.Next(0, users.Count)],
			     new Content(f.Lorem.Sentence(7)),
			     posts[Randomizer.Seed.Next(0, posts.Count)]));
    
     private Faker<Post> CreatePostFaker(IList<User> users)
	     => new Faker<Post>()
		     .CustomInstantiator(f => new Post(
			     users[Randomizer.Seed.Next(0, users.Count)],
			     new Content(f.Lorem.Sentence(20)),
			     new Media($"Original_{Guid.NewGuid()}.jpg",
				     "image/jpeg",
				     f.Image.PicsumUrl())));
    
    private Task<User[]> CreateUsers(Faker<User> userFaker)
	    => Task.WhenAll(userFaker
	                    .Generate(UsersCount)
	                    .Select(CreateUser));
    
    private Task<Comment[]> CreateComments(Faker<Comment> commentFaker)
	    => Task.WhenAll(commentFaker
	                    .Generate(CommentsCount)
	                    .Select(CreateComment));
    
    private Task<Post[]> CreatePosts(Faker<Post> postFaker)
	    => Task.WhenAll(postFaker
	                    .Generate(PostsCount)
	                    .Select(CreatePost));
    
    private Task CreateFollows(User[] users)
    {
	    ISet<(UserId, UserId)> followsSet = new HashSet<(UserId, UserId)>();
	    return Task.WhenAll(Enumerable.Range(0, FollowsCount).Select(_ =>
	    {
		    var followerId = users[Randomizer.Seed.Next(0, 30)].Id;
		    var followeeId = users[Randomizer.Seed.Next(0, 30)].Id;
		    if (followsSet.Contains((followerId, followeeId)))
			    return Task.CompletedTask;
		    followsSet.Add((followerId, followeeId));
		    return CreateFollow(followerId, followeeId);
	    }));
    }
    
    private Task<Liked[]> CreateLikes(
	    IReadOnlyList<User> users,
	    IReadOnlyList<Post> posts)
    {
	    ISet<(UserId, Guid)> likesSet = new HashSet<(UserId, Guid)>();
	    
	    var likeTasks = Enumerable.Range(0, LikesCount).Select(_ =>
	    {
		    var userId = users[Randomizer.Seed.Next(0, 30)].Id;
		    var postId = posts[Randomizer.Seed.Next(0, 300)].Id;
			  
		    if (likesSet.Contains((userId, postId)))
		    {
			    return Task.FromResult((Liked) null);
		    }
			  
		    likesSet.Add((userId, postId));
		    return CreateLike(userId, postId);
	    });
	    
	    return Task.WhenAll(likeTasks);
    }

    private Task<Liked> CreateLike(UserId userId, Guid postId)
		=> _likes.CreateAsync(userId, postId, new Liked(_dateTime.Now));
    
    private Task<Comment> CreateComment(Comment comment)
		=> _comments.SaveAsync(comment);
    
    private Task CreateFollow(UserId followerId, UserId followeeId)
	    => followerId == followeeId
		    ? Task.CompletedTask
		     : _follows.CreateAsync(followerId, followeeId, new Domain.Relationships.Follows(_dateTime.Now));

    private Task<Post> CreatePost(Post post)
	    => _posts.SaveAsync(post);
    
    private async Task<User> CreateUser(User user)
    {
	    var userPhoto = user.Profile.Photo;
	    var newUser = await _users.SaveAsync(user);
	    
	    await _media.SaveProfileMediaAsync(newUser.Id, userPhoto);
	    return newUser;
    }
}