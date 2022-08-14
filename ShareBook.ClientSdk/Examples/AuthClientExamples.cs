using ShareBook.ClientSdk.Clients;
using ShareBook.ClientSdk.Clients.Requests;

namespace ShareBook.ClientSdk.Examples;

public class AuthClientExamples : BaseClientExample<IAuthClient>
{
	public AuthClientExamples()
		: base(string.Empty) { }
	
	public async Task LoginExample(AuthenticateRequest request)
	{
		var response = await _client.Authenticate(request);
		Console.WriteLine($"User Id: {response.Content.Id}");
		
		if (response.IsSuccessStatusCode)
		{
			_authToken = response.Content.Token;
		}
	}
	
	public async Task SignUpExample(SignUpRequest request)
	{
		var response = await _client.SignUp(request);
		Console.WriteLine($"User Id: {response.Content.Id}");
		
		if (response.IsSuccessStatusCode)
		{
			_authToken = response.Content.Token;
		}
	}
	public async Task PersonalDetailsExample()
	{
		var response = await _client.PersonalDetails(_authToken);
		
		if (response.IsSuccessStatusCode)
		{
			Console.WriteLine(response.Content.Name);
		}
	}
}