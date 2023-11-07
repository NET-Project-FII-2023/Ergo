
using Ergo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ErgoContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Ergo.Domain.Entities.Task> Tasks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost; Database=Ergo; Username=postgres; Password=1234");
    }

}