using ClassLib.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClassLib.Entities;

public class AccountProfile
{
    [Key]
    public Guid id { get; set; }

    [ForeignKey("user_id")]
    public Guid user_id { get; set; }

    [MaxLength(100)]
    public string email { get; set; }
    [MaxLength(10)]
    public string phone_no { get; set; }
    public string profile_image { get; set; }
    public string occupation { get; set; }

    [MaxLength(3)]
    public string gender { get; set; }

    public DateTime create_date { get; set; }
    
    [MaxLength(50)] 
    public string create_by { get; set; }
    
    public DateTime? update_date { get; set; }
    [MaxLength(50)] 
    public string? update_by { get; set; }
}
