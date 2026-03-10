using ClassLib.Data.Entities;
using ClassLib.Entities;
using ClassLib.Models.Comm;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLib.Model.Comm.Authentication;

public class LoginResponse : Response<LoginInfomations>
{
    public TokenDetails? TokenDetails { get; set; }
    public LoginDataDto UserData { get; set; }
}

public class LoginDataDto
{
    public Guid UserID { get; set; }
    public string Username { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
     
}

public class TokenDetails
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
     
}