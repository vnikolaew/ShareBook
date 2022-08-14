using Refit;

namespace ShareBook.ClientSdk.Examples;
public abstract class BaseClientExample<TClient>
	where TClient : class
{
	protected readonly TClient _client;
	protected const string baseApiUrl = "https://localhost:7064";
	protected string _authToken;
	protected BaseClientExample(string authToken)
	{
		_client = RestService.For<TClient>(baseApiUrl, new RefitSettings
		{
			ContentSerializer = new SystemTextJsonContentSerializer()
		});
		
		_authToken = authToken;
	}
}