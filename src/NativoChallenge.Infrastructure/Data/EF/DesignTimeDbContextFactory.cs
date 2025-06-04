using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace NativoChallenge.Infrastructure.Data.EF;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Ruta al proyecto de WebAPI (startup project)
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../NativoChallenge.WebAPI");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("NativoConnectionString"));

        return new AppDbContext(optionsBuilder.Options);
    }
}
