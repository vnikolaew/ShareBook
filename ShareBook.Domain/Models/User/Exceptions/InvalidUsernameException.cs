using ShareBook.Domain.Common;

namespace ShareBook.Domain.Models.User.Exceptions;

public class InvalidUsernameException : ShareBookException
{
  public InvalidUsernameException(string message)
    => Message = message;

  public InvalidUsernameException() { }
}