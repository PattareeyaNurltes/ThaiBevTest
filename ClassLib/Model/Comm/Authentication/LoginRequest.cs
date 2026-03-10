using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLib.Model.Comm.Authentication;

public class LoginRequest
{
    public LoginRequestMeta meta { get; set; }
    public LoginRequestData data { get; set; }
}

public class LoginRequestMeta
{
    public Guid RequestID { get; set; }
    public string API_KEY { get; set; }
}

public class LoginRequestData
{
    public string Username { get; set; }
    public string Password { get; set; }
}