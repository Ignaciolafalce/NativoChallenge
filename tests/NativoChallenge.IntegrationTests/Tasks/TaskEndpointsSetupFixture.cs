using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NativoChallenge.Application.Auth.DTOs;
using NativoChallenge.Domain.Entities.Identity;
using NativoChallenge.Domain.Enums;
using NativoChallenge.Infrastructure.Data.EF;
using NativoChallenge.WebAPI;
using NativoChallenge.WebAPI.Common;
using NativoChallenge.WebAPI.Common.Auth;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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

            SeedUsersAsync(sp).GetAwaiter().GetResult();
        });
    }

    //protected override void ConfigureClient(HttpClient client)
    //{
    //    base.ConfigureClient(client);
    //    //var tmpClient = new HttpClient();
    //    //tmpClient.BaseAddress = client.BaseAddress; // maybe it can be refactor
    //    var loginRequest = new
    //    {
    //        userName = "admin",
    //        password = "Admin123!"
    //    };

    //    var response = client.PostAsJsonAsync("/auth/token", loginRequest).GetAwaiter().GetResult();
    //    var result = response.Content.ReadFromJsonAsync<ApiResponse<TokenResult>>().GetAwaiter().GetResult();


    //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result!.Data!.AccessToken);
    //}

    public async Task SeedUsersAsync(IServiceProvider services)
    {
        var scope = services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        var adminRole = new ApplicationRole { Name = AppRoles.Admin };
        if (!await roleManager.RoleExistsAsync(adminRole.Name))
        {
            await roleManager.CreateAsync(adminRole);
        }

        var adminUser = new ApplicationUser
        {
            UserName = "admin",
            Email = "admin@nativo.com"
        };

        if (await userManager.FindByNameAsync(adminUser.UserName) is not null)
        {
            await userManager.CreateAsync(adminUser, "Admin123!");
            await userManager.AddToRoleAsync(adminUser, AppRoles.Admin);
        }
    }

    public async Task AuthenticateAdminAsync(HttpClient client)
    {
        var loginRequest = new { userName = "admin", password = "Admin123!" };

        var response = await client.PostAsJsonAsync("/auth/token", loginRequest);
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<TokenResult>>();
       
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result!.Data!.AccessToken);
    }

    public async Task<List<Entities.Task>> DefaultSeedTasksAsync()
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