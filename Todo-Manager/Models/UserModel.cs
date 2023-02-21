using System.ComponentModel.DataAnnotations;
using Todo_Manager.Enums;

namespace Todo_Manager.Models;

public class UserModel : BaseModel
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public UserRole Role { get; set; }
}