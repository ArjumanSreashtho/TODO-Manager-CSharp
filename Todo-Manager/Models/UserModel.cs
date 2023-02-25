using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Todo_Manager.Models;

[Index(nameof(UserModel.Username), IsUnique=true)]
public class UserModel : BaseModel
{

    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    [DefaultValue("USER")]
    public string Role { get; set; }
    
    public virtual ICollection<UserTaskModel> UserTasks { get; set; }
}