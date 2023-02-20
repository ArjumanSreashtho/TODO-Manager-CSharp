using System.ComponentModel.DataAnnotations;

namespace Todo_Manager.Models;

public abstract class BaseModel
{
    [Key]
    public Guid Id { set; get; }
    public DateTime CreatedAt { set; get; }
    public DateTime UpdatedAt { set; get; }
}