using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLib.Data.Entities;

public class LoginInfomations
{
    [Key]
    public int id { get; set; }
    
    public Guid user_id { get; set; }

    [MaxLength(20)]
    public string login_status { get; set; }
    public DateTime last_login_date { get; set; }

    [ForeignKey("user_id")] 
    public Accounts? Account { get; set; }

}
