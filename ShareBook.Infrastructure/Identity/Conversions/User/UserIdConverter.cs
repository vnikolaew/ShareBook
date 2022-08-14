using Newtonsoft.Json;
using ShareBook.Domain.Models.User;

namespace ShareBook.Infrastructure.Identity.Conversions.User;

public class UserIdConverter : ValueObjectConverter<UserId>
{
	public override UserId? ReadJson(JsonReader reader, Type objectType, UserId? existingValue, bool hasExistingValue,
		JsonSerializer serializer) => new(Guid.Parse(reader.Value as string));
}