using Newtonsoft.Json;
using ShareBook.Domain.Models.Profile;

namespace ShareBook.Infrastructure.Identity.Conversions.Profile;

public class BioConverter : ValueObjectConverter<Bio>
{
	public override Bio? ReadJson(
		JsonReader reader,
		Type objectType,
		Bio? existingValue,
		bool hasExistingValue,
		JsonSerializer serializer)
		=> new(reader.Value as string);
}