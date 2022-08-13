using ShareBook.Domain.Common;
using ShareBook.Domain.Models.Post.Exceptions;
using static ShareBook.Domain.Models.ModelConstants.Post;

namespace ShareBook.Domain.Models.Post;
public class Content : ValueObject
{
  public string Value { get; }

  public Content(string value)
  {
    Guard.ForStringLength<InvalidContentException>(value, MinContentLength, MaxContentLength);
    Value = value;
  }

  public static implicit operator string(Content content)
    => content.Value;

  public static implicit operator Content(string content)
    => new(content);

  public override string ToString() => Value;
}