using System.Text.RegularExpressions;

namespace ShareBook.Domain.Models;

public static class ModelConstants
{
  public static class User
  {
    public const int MinUsernameLength = 3;
    public const int MaxUsernameLength = 50;
    
    public const int MinEmailLength = 5;
    public const int MaxEmailLength = 200;
    
    public const int MinPasswordLength = 6;
    public const int MaxPasswordLength = 100;
    
    public static readonly Regex ValidEmailRegex = new Regex("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$");
  }

  public static class Post
  {
    public const int MinContentLength = 3;
    public const int MaxContentLength = 800;
  }

  public static class Profile
  {
    public const int MinBioLength = 0;
    public const int MaxBioLength = 1000;
    
    public const int MinProfilePhotoUrlLength = 0;
    public const int MaxProfilePhotoUrlLength = 200;
    
    public const int MinFullNameLength = 0;
    public const int MaxFullNameLength = 200;
  }
}