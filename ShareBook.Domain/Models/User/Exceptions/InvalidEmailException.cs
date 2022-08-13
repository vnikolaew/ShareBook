using ShareBook.Domain.Common;

namespace ShareBook.Domain.Models.User.Exceptions;
public class InvalidEmailException : ShareBookException
{
  public InvalidEmailException(string message)
    => Message = message;
  public InvalidEmailException() { }
}