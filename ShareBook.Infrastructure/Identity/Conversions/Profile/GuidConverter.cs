using Newtonsoft.Json;

namespace ShareBook.Infrastructure.Identity.Conversions.Profile;

public class GuidConverter : JsonConverter<Guid>
{
	public override void WriteJson(JsonWriter writer, Guid value, JsonSerializer serializer)
		=> writer.WriteValue(value);

	public override Guid ReadJson(JsonReader reader, Type objectType, Guid existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		var value = reader.Value as string ?? string.Empty;
		return Guid.Parse(value);
	}
}