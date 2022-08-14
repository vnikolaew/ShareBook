using Refit;
using ShareBook.ClientSdk.Clients.Posts;
using ShareBook.ClientSdk.Clients.Posts.Requests;

namespace ShareBook.ClientSdk.Examples;

public class PostClientExamples : BaseClientExample<IPostClient>
{
	public PostClientExamples(string authToken)
		: base(authToken) { }

	public async Task Create(Stream file, string content)
	{
		var response = await _client.Create(new StreamPart(file, string.Empty), content);
		Console.WriteLine(response.ReasonPhrase);
	}

	public async Task Mine()
	{
		var response = await _client.Mine(_authToken);

		foreach (var post in response.Content.Posts)
		{
			Console.WriteLine($"[{post.CreatedOn}] {post.Id}: {post.Content}");
		}
	}
	
	public async Task ByUser(Guid userId)
	{
		var response = await _client.ByUser(userId, _authToken);

		foreach (var post in response.Content.Posts)
		{
			Console.WriteLine($"[{post.CreatedOn}] {post.Id}: {post.Content}");
		}
	}
	
	public async Task Details(Guid postId)
	{
		var response = await _client.Details(postId, _authToken);

		var post = response.Content;
		Console.WriteLine($"{post.Id}: {post.Content} by {post.Author}");
		
		foreach (var like in post.Likes)	
		{
			Console.WriteLine($"[{like.At}] by {like.Username}");
		}
		
		foreach (var comment in post.Comments)
		{
			Console.WriteLine($"[{comment.CreatedOn}] {comment.Content} by {comment.Author}");
		}
	}

	public async Task Edit(EditPostRequest request, Guid postId)
	{
		var response = await _client.Edit(request, postId, _authToken);
		Console.WriteLine(response.ReasonPhrase);
	}
	
	public async Task Delete(Guid postId)
	{
		var response = await _client.Delete(postId, _authToken);
		Console.WriteLine(response.ReasonPhrase);
	}
}