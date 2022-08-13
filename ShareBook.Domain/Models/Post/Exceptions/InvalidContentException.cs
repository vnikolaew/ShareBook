using ShareBook.Domain.Common;

namespace ShareBook.Domain.Models.Post.Exceptions;
public class InvalidContentException : ShareBookException
{
  public InvalidContentException(string message) => Message = message;
  public InvalidContentException() { }
}