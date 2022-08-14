using Newtonsoft.Json;
using ShareBook.Domain.Models.Profile;

namespace ShareBook.Infrastructure.Identity.Conversions.Profile;

public class FullNameConverter : ValueObjectConverter<FullName>
{
	public override FullName? ReadJson(JsonReader reader, Type objectType, FullName? existingValue, bool hasExistingValue,
		JsonSerializer serializer)
		=> new(reader.Value as string);
}