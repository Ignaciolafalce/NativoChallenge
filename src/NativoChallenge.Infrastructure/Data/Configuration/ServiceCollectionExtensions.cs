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
            var useInMemoryDb = bool.Parse(configuration.GetSection("UseInMemoryDb").Value?.ToLower() ?? "false");
            services.AddDbContext<AppDbContext>(options =>
            {
                if (useInMemoryDb)
                {
                    options.UseInMemoryDatabase("ChallengeDb");
                }

                if (!useInMemoryDb)
                {
                    var appDbContextType = typeof(AppDbContext);
                    var assembly = Assembly.GetAssembly(appDbContextType);
                    if (assembly == null)
                    {
                        throw new InvalidOperationException($"Unable to retrieve assembly for type {appDbContextType.FullName}");
                    }

                    options.UseSqlServer(configuration.GetConnectionString("NativoConecctionString"), opt => opt.MigrationsAssembly(assembly.FullName));
                }
            });

            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IApplicationMarker).Assembly));

            return services;
        }
    }
}
