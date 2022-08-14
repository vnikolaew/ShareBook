using Newtonsoft.Json;
using ShareBook.Domain.Common;

namespace ShareBook.Infrastructure.Identity.Conversions;

public abstract class ValueObjectConverter<T> : JsonConverter<T>
	where T : ValueObject
{
	public override void WriteJson(
		JsonWriter writer,
		T? value,
		JsonSerializer serializer)
		=> writer.WriteValue(value.ToString());

	public override abstract T? ReadJson(
		JsonReader reader,
		Type objectType,
		T? existingValue,
		bool hasExistingValue,
		JsonSerializer serializer);
}