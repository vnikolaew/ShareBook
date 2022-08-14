using Newtonsoft.Json;
using ShareBook.Domain.Models.User;

namespace ShareBook.Infrastructure.Identity.Conversions.User;

public class PasswordConverter : ValueObjectConverter<Password>
{
	public override Password? ReadJson(JsonReader reader, Type objectType, Password? existingValue, bool hasExistingValue,
		JsonSerializer serializer) => new(reader.Value as string);
}