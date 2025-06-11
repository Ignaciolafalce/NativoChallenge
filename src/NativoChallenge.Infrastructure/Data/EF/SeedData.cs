using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NativoChallenge.Domain.Entities.Identity;
using NativoChallenge.Domain.Entities.Task;
using NativoChallenge.Domain.Enums;
using Entities = NativoChallenge.Domain.Entities.Task;

namespace NativoChallenge.Infrastructure.Data.EF;

public static class SeedData
{
    public static async System.Threading.Tasks.Task SeedTasksAsync(AppDbContext dbContext)
    {
        if (await dbContext.Tasks.AnyAsync())
        {
            return;
        }

        var sampleTasks = new List<Entities.Task>
        {
            new("Hacer Challenge Nativo", "Usar Clean Architecture, Principios (Arq/Diseño), Patrones (Arq/diseño)... etc", DateTime.UtcNow.AddDays(1), TaskPriority.High),
            new("Estudiar .NET", "Repasar patrones y arquitectura", DateTime.UtcNow.AddDays(2), TaskPriority.Medium),
            new("Hacer la VTV", "Llevar el auto al taller antes de viernes", DateTime.UtcNow.AddDays(3), TaskPriority.Low)
        };

        dbContext.Tasks.AddRange(sampleTasks);
        await dbContext.SaveChangesAsync();
    }

    public static async System.Threading.Tasks.Task SeedAdminUsersAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        var role = "Admin";

        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new ApplicationRole(role));
        }

        var defaultUser = new ApplicationUser
        {
            UserName = "admin",
            Email = "admin@nativo.com"
        };

        if (await userManager.FindByNameAsync(defaultUser.UserName) is null)
        {
            await userManager.CreateAsync(defaultUser, "Admin123!"); // contraseñas seguras por defecto
            await userManager.AddToRoleAsync(defaultUser, role);
        }
    }

}