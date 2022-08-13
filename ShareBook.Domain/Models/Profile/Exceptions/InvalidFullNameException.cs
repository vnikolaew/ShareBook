using ShareBook.Domain.Common;

namespace ShareBook.Domain.Models.Profile.Exceptions;

public class InvalidFullNameException : ShareBookException
{
  public InvalidFullNameException(string message)
    => Message = message;

  public InvalidFullNameException() { }
}