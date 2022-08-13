using ShareBook.Domain.Common;

namespace ShareBook.Domain.Models.User.Exceptions;

public class EmptyUserIdException : ShareBookException
{
  public EmptyUserIdException(string message)
    => Message = message;

  public EmptyUserIdException() { }
}