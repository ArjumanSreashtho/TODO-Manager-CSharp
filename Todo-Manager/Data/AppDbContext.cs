using Microsoft.EntityFrameworkCore;
using Todo_Manager.Models;

namespace Todo_Manager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public virtual DbSet<TaskModel> Tasks { get; set; }
    public virtual DbSet<UserModel> Users { get; set; }
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var insertedEntries = this.ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity);

        foreach(var insertedEntry in insertedEntries)
        {
            //If the inserted object is an Auditable. 
            if(insertedEntry is BaseModel baseEntity)
            {
                baseEntity.CreatedAt = DateTime.UtcNow;
                baseEntity.UpdatedAt = DateTime.UtcNow;
            }
        }

        var modifiedEntries = this.ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Modified)
            .Select(x => x.Entity);

        foreach (var modifiedEntry in modifiedEntries)
        {
            //If the inserted object is an Auditable. 
            var baseEntity = modifiedEntry as BaseModel;
            if (baseEntity != null)
            {
                baseEntity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserModel>().HasData(new UserModel()
        {
            Id = Guid.NewGuid(),
            Name = "Arjuman Sreashtho",
            Username = "Arjuman",
            Password = "$2a$12$nTQDx/njEA9wGrX1P845CenRjAf/pREoHqflQrS3EgIkExEynh3/O",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Role = "ADMIN"
        });
    }
}