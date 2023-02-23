using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Todo_Manager.Models;
public class TaskModel : BaseModel
{
    [Required]
    public string Title { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public bool Completed { get; set; }
}