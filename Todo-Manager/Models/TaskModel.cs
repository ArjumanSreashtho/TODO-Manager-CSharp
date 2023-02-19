using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Todo_Manager.Models;
public class TaskModel : BaseModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool Completed { get; set; }
}