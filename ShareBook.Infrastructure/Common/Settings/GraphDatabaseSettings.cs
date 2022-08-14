namespace ShareBook.Infrastructure.Common.Settings;
public class GraphDatabaseSettings
{
	public string Username { get; set; }
	public string Password { get; set; }
	public string BoltEndpoint { get; set; }
	public string DefaultDatabase { get; set; }
	public bool UseDataSeeding { get; set; }
}