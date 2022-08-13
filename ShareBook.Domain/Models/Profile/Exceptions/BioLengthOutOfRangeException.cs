using ShareBook.Domain.Common;

namespace ShareBook.Domain.Models.Profile.Exceptions;

public class BioLengthOutOfRangeException : ShareBookException
{
  public BioLengthOutOfRangeException(string message)
    => Message = message;

  public BioLengthOutOfRangeException() { }
}