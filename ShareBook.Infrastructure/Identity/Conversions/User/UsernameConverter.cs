using Newtonsoft.Json;
using ShareBook.Domain.Models.User;

namespace ShareBook.Infrastructure.Identity.Conversions.User;

public class UsernameConverter : ValueObjectConverter<Username>
{
	public override Username? ReadJson(JsonReader reader, Type objectType, Username? existingValue, bool hasExistingValue,
		JsonSerializer serializer) => new(reader.Value as string);
}