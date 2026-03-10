using ClassLib.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassLib.Model.Comm.Users;

public class UserManagementRequest
{
    public UserManagementRequestMeta meta { get; set; }
    public UserManagementRequestData data { get; set; }
}


public class UserManagementRequestMeta
{
    public Guid RequestID { get; set; }
    public string API_KEY { get; set; }
}

public class UserManagementRequestData
{
    public required Guid UserID { get; set; }
    public string? Password { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? UpdateBy { get; set; }
    public string? Email { get; set; }
    public string? Phone_no { get; set; }
    public string? Profile_image { get; set; }
    public string? Occupation { get; set; }
    public string? Gender { get; set; }
}