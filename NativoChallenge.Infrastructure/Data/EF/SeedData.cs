using Microsoft.EntityFrameworkCore;
using NativoChallenge.Domain.Enums;
using Entities = NativoChallenge.Domain.Entities;

namespace NativoChallenge.Infrastructure.Data.EF;

public static class SeedData
{
    public static async Task SeedAsync(AppDbContext dbContext   )
    {
        if (await dbContext .Tasks.AnyAsync())
        {
            return;
        }

        var sampleTasks = new List<Entities.Task>
        {
            new("Hacer Challenge Nativo", "Usar Clean Architecture, Principios (Arq/Diseño), Patrones (Arq/diseño)... etc", DateTime.UtcNow.AddDays(1), TaskPriority.High),
            new("Estudiar .NET", "Repasar patrones y arquitectura", DateTime.UtcNow.AddDays(2), TaskPriority.Medium),
            new("Hacer la VTV", "Llevar el auto al taller antes de viernes", DateTime.UtcNow.AddDays(3), TaskPriority.Low)
        };

        dbContext   .Tasks.AddRange(sampleTasks);
        await dbContext .SaveChangesAsync();
    }
}