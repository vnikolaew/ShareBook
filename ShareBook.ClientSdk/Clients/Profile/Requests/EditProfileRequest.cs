namespace ShareBook.ClientSdk.Clients.Profile.Requests;

public class EditProfileRequest
{
	public string Bio { get; set; }
	public Gender Gender { get; set; }
}

public enum Gender : sbyte
{
	Male,
	Female,
	Other
}
