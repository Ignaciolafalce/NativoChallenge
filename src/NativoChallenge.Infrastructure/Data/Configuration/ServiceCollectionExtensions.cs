using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NativoChallenge.Application;
using NativoChallenge.Application.Interfaces;
using NativoChallenge.Domain.Entities.Identity;
using NativoChallenge.Domain.Interfaces;
using NativoChallenge.Infrastructure.Data.EF;
using NativoChallenge.Infrastructure.Data.Repositories;
using NativoChallenge.Infrastructure.Services;
using System.Reflection;

namespace NativoChallenge.Infrastructure.Data.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            // Services 
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // Repositories
            services.AddScoped<ITaskRepository, TaskRepository>();

            // MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IApplicationMarker).Assembly));

            // Identity Core
            services.AddIdentityCore<ApplicationUser>()
                    .AddRoles<ApplicationRole>()
                    .AddEntityFrameworkStores<AppDbContext>();

            // Authentication and Authorization
            services.AddHttpContextAccessor()
                    .AddAuthorization()
                    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => { 

                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = configuration["Jwt:Issuer"],
                            ValidAudience = configuration["Jwt:Audience"],
                            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                        };

                    });

            // EF
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
