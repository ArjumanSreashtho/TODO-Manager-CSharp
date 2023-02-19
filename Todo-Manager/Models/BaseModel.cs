using System.ComponentModel.DataAnnotations;

namespace Todo_Manager.Models;

public class BaseModel
{
    public BaseModel()
    {
        Id = new Guid();
    }
    [Key]
    public Guid Id { set; get; }
    public DateTime CreatedAt { set; get; } = DateTime.UtcNow;
    public DateTime UpdatedAt { set; get; } = DateTime.UtcNow;
}