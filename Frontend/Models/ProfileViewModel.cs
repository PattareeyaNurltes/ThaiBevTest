namespace Frontend.Models;

public class ProfileViewModel
{
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public string Email { get; set; }
    public string Phone_No{ get; set; }
    public string Profile_Image{ get; set; }
    public DateTime DateOfBirth{ get; set; }
    public string Occupation { get; set; }
    public string Gender { get; set; }

}
