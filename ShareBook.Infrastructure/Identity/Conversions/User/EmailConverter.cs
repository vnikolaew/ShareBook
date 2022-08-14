using Newtonsoft.Json;
using ShareBook.Domain.Models.User;

namespace ShareBook.Infrastructure.Identity.Conversions.User;

public class EmailConverter : ValueObjectConverter<Email>
{
	public override Email? ReadJson(JsonReader reader, Type objectType, Email? existingValue, bool hasExistingValue,
		JsonSerializer serializer) => new(reader.Value as string);
}