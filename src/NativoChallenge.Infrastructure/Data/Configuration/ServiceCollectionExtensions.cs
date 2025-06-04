using NativoChallenge.Domain.Interfaces;
using NativoChallenge.Application;
using NativoChallenge.Infrastructure.Data.EF;
using NativoChallenge.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace NativoChallenge.Infrastructure.Data.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IApplicationMarker).Assembly));

            var useInMemoryDb = bool.Parse(configuration.GetSection("UseInMemoryDb").Value?.ToLower() ?? "false");
            services.AddDbContext<AppDbContext>(options =>
            {
                    Console.WriteLine("Database - Using in memory?: " + configuration.GetValue<bool>("UseInMemoryDb"));
                if (useInMemoryDb)
                {
                    options.UseInMemoryDatabase("ChallengeDb");
                }

                if (!useInMemoryDb)
                {
                    options.UseSqlServer(configuration.GetConnectionString("NativoConnectionString"));
                    Console.WriteLine("Database - ConnectionString: " + configuration.GetConnectionString("NativoConnectionString"));
                }
            });


            return services;
        }
    }
}
