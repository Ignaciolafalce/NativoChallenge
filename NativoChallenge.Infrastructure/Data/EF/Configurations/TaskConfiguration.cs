using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entities = NativoChallenge.Domain.Entities;

namespace NativoChallenge.Infrastructure.Data.EF.Configurations;

internal class TaskConfiguration : IEntityTypeConfiguration<Entities.Task>
{
    public void Configure(EntityTypeBuilder<Entities.Task> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Title).IsRequired().HasMaxLength(100); // tmp max length
        builder.Property(t => t.Description).HasMaxLength(500); // tmp max length
        builder.Property(t => t.ExpirationDate);
        builder.Property(t => t.Priority);
        builder.Property(t => t.State);
    }
}
