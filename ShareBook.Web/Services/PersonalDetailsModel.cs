using ShareBook.Application.Common.Contracts;

namespace ShareBook.Web.Services;

public class PersonalDetailsModel : IPersonalDetails
{
	public string Name { get; init; }
	public string Id { get; init; }
	public IEnumerable<Claim> Claims { get; set; }

	public class Claim
	{
		public string Type { get; set; }
		public string Value { get; set; }
	}
}