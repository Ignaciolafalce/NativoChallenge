using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NativoChallenge.Domain.Entities;
using NativoChallenge.Domain.Entities.Identity;
using NativoChallenge.Infrastructure.Data.EF.Configurations;
using Entities = NativoChallenge.Domain.Entities;

namespace NativoChallenge.Infrastructure.Data.EF;

public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public DbSet<Entities.Task.Task> Tasks { get; set; }

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
