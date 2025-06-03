using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NativoChallenge.Domain.Enums;
using NativoChallenge.Infrastructure.Data.EF;
using NativoChallenge.WebAPI;
using Entities = NativoChallenge.Domain.Entities;

namespace NativoChallenge.IntegrationTests.Tasks;

public class TaskEndpointsSetupFixture : WebApplicationFactory<Program>
{
    private readonly string _databaseName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // This implementetion could be improve probably with testing databae(sql? postgres?) for diferentes enviroments
            // Delete AppDbContext prev registered
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            // Register AppDbContext in memory for test
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase(_databaseName);
            });

            using var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        });
    }

    public async Task<List<Entities.Task>> DefaultSeedAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Tasks.RemoveRange(dbContext.Tasks); // limpiar por las dudas
        await dbContext.SaveChangesAsync();

        var sampleTasks = new List<Entities.Task>
        {
            new("Hacer Challenge Nativo", "Usar Clean Architecture, Principios...", DateTime.UtcNow.AddDays(1), TaskPriority.High),
            new("Estudiar .NET", "Repasar patrones y arquitectura", DateTime.UtcNow.AddDays(2), TaskPriority.Medium),
            new("Hacer la VTV", "Llevar el auto al taller", DateTime.UtcNow.AddDays(3), TaskPriority.Low)
        };

        dbContext.Tasks.AddRange(sampleTasks);
        await dbContext.SaveChangesAsync();

        return sampleTasks;
    }
}