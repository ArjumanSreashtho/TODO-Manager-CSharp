using Microsoft.EntityFrameworkCore;
using Todo_Manager.Models;

namespace Todo_Manager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public virtual DbSet<TaskModel> Tasks { get; set; }
}