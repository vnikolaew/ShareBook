using ShareBook.Domain.Common;

namespace ShareBook.Domain.Models.Media;
public class Media : AuditableEntity<Guid>
{
  public string MediaName { get; private set; }
  public string MediaType { get; private set; }
  public string AbsoluteUrl { get; private set; }

  public Media(string mediaName, string mediaType, string absoluteUrl)
  {
    MediaName = mediaName;
    MediaType = mediaType;
    AbsoluteUrl = absoluteUrl;
  }

  public Media() { }
}