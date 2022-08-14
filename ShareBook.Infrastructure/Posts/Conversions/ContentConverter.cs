using Newtonsoft.Json;
using ShareBook.Domain.Models.Post;
using ShareBook.Infrastructure.Identity.Conversions;

namespace ShareBook.Infrastructure.Posts.Conversions;

public class ContentConverter : ValueObjectConverter<Content>
{
	public override Content? ReadJson(JsonReader reader, Type objectType, Content? existingValue, bool hasExistingValue,
		JsonSerializer serializer) => new(reader.Value as string);
}