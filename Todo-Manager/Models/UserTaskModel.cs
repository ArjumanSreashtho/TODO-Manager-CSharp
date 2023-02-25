using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo_Manager.Models;

public class UserTaskModel
{
    public Guid UserId { get; set; }
    public virtual UserModel User { get; set; }
    public Guid TaskId { get; set; }
    public virtual TaskModel Task { get; set; }
}