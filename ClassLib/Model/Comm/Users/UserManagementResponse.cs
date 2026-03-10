using ClassLib.Data.Entities;
using ClassLib.Entities;
using ClassLib.Models.Comm;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLib.Model.Comm.Users;

public class UserManagementResponse : Response<Accounts>
{
    public AccountProfile? Profile { get; set; }
}
