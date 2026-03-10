namespace ClassLib.Models.Comm;

public class RegistrationRequest
{

    public RegistrationRequestMeta meta { get; set; }
    public RegistrationRequestData data { get; set; }
}


public class RegistrationRequestMeta 
{
     public Guid RequestID { get; set; }
     public string API_KEY { get; set; }

}

public class RegistrationRequestData 
{
     public string UserName { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; }
}