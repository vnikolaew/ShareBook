using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ShareBook.Domain.Common;
public static class Guard
{
  public static void AgainstEmptyString<TException>(string input, string name = "Value") where TException : ShareBookException, new()
  {
    if (!string.IsNullOrEmpty(input))
      return;
    
    ThrowException<TException>($"The {name} must not be null or empty.");
  }

  public static void ForValidRegex<TException>(string input, Regex validRegex, string name = "Value") where TException : ShareBookException, new()
  {
    if (validRegex.IsMatch(input))
      return;
    
    ThrowException<TException>($"The {name} didn't pass the supplied regex: {validRegex}");
  }

  public static void AgainstOutOfRange<TException>(
    int value,
    int minValue,
    int maxValue,
    string name = "Value")
    where TException : ShareBookException, new()
  {
    if (value >= minValue && value <= maxValue)
    {
      return;
    }
    
    ThrowException<TException>($"The value {name} must be in the range {minValue} and {maxValue}.");
  }

  public static void AgainstOutOfRange<TException>(
    decimal value,
    decimal minValue,
    decimal maxValue,
    string name = "Value")
    where TException : ShareBookException, new()
  {
    if (value >= minValue && value <= maxValue)
    {
      return;
    }
    
    ThrowException<TException>($"The value {name} must be in the range {minValue} and {maxValue}.");
  }

  public static void ForStringLength<TException>(
    string input,
    int minLength,
    int maxLength,
    string name = "Value")
    where TException : ShareBookException, new()
  {
    if (input == null)
    {
      ThrowException<TException>($"The {name} must not be null or empty.");
    }
    
    if (input.Length >= minLength && input.Length <= maxLength)
    {
      return;
    }
    
    ThrowException<TException>($"The {name} must be between {minLength} and {maxLength} characters.");
  }

  public static void Against<TException>(
    object? actualValue,
    object? unexpectedValue,
    string name = "Value")
    where TException : ShareBookException, new()
  {
    if (!actualValue.Equals(unexpectedValue) || actualValue != unexpectedValue)
    {
      return;
    }
    
    ThrowException<TException>($"{name} must not be {unexpectedValue}.");
  }

  private static void ThrowException<TException>(string message)
    where TException : ShareBookException, new()
    => throw new TException { Message = message };
}