using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLib.Data.Entities;

public class Accounts
{
    [Key] 
    public Guid user_id { get; set; }
    
    [Required]
    [MaxLength(100)] 
    public string firstname { get; set; }
    
    [Required]
    [MaxLength(100)] 
    public string lastname { get; set; }

    [MaxLength(100)]
    public string username { get; set; }
    public string password { get; set; }
    public string password_salt { get; set; }

    [Required]
    [Column(TypeName = "date")] 
    public DateTime date_of_birth { get; set; }

    [Required]
    [MaxLength(500)] public string address { get; set; } //Normally, I usually separate the address fields into house number, alley, road, district, sub-district, etc.
    

    public bool is_active { get; set; }

    public string access_token { get; set; }
    public string refresh_token { get; set; }

    [Required] 
    public DateTime create_date { get; set; }
    
    [Required]
    [MaxLength(50)] 
    public string create_by { get; set; }
    
    public DateTime? update_date { get; set; }
    [MaxLength(50)] 
    public string? update_by { get; set; }

}
