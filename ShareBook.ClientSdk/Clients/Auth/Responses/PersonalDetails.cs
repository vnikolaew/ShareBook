namespace ShareBook.ClientSdk.Clients.Responses;

public class PersonalDetails
{
	public string Name { get; set; }
	public string Id { get; set; }
	public IEnumerable<Claim> Claims { get; set; }
	public class Claim
	{
		public string Type { get; set; }
		public string Value { get; set; }
	}
}