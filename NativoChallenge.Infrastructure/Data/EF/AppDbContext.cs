using NativoChallenge.Infrastructure.Data.EF.Configurations;
using Microsoft.EntityFrameworkCore;
using Entities = NativoChallenge.Domain.Entities;

namespace NativoChallenge.Infrastructure.Data.EF;

public class AppDbContext:DbContext
{
    public DbSet<Entities.Task> Tasks { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Task entity configuration
        modelBuilder.ApplyConfiguration(new TaskConfiguration());
    }
}
