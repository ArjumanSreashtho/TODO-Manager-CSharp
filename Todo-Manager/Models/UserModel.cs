using System.ComponentModel.DataAnnotations;

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
    public string Role { get; set; }
}