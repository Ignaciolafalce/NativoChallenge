
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using NativoChallenge.Application.Configuration;
using NativoChallenge.Domain.Entities.Identity;
using NativoChallenge.Infrastructure.Data.Configuration;
using NativoChallenge.Infrastructure.Data.EF;
using NativoChallenge.WebAPI.Common.Auth;
using NativoChallenge.WebAPI.Endpoints;
using NativoChallenge.WebAPI.Extensions;
using OpenTelemetry.Trace;
using Serilog;

namespace NativoChallenge.WebAPI;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddSerilogConfiguration();

        #region Services 

        Log.Information("Configuring Services");

        // Application and Infrastructure services
        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddAppAuthorization();

        // OpenApi and Swagger services
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Nativo Challenge API",
                Version = "v1",
                Description = "API para gestión de tareas usando Clean Architecture.",
                Contact = new OpenApiContact
                {
                    Name = "Ignacio Lafalce",
                    Email = "nacho_lafalce@hotmail.com"
                }
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme
                {
                   Reference = new OpenApiReference{ Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                },
                new string[] { }
            }
          });

        });
       
        // Openlemetry services and configuration for tracing
        builder.Services.AddOpenTelemetry()
                        .WithTracing(tracing =>
                        {
                            tracing.AddAspNetCoreInstrumentation()
                                   .AddHttpClientInstrumentation()
                                   .AddSqlClientInstrumentation()
                                   .AddConsoleExporter();
                        });

        #endregion 


        var app = builder.Build();

        // Seed Task data
        Log.Information("Seeding Task Data");
        SeedTasksData(app);

        #region Http Request Pipeline Configuration - Middlewares

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            //app.UseSwagger();
            //app.UseSwaggerUI();
        }

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Nativo Challenge API v1");
            options.DocumentTitle = "Nativo Challenge";
        });

        app.UseHttpsRedirection();

        app.UseCustomExceptionHandler(); // Custom exception handler middleware

        // Redirect root path to Swagger UI
        app.MapGet("/", () => Results.Redirect("/swagger"))
           .ExcludeFromDescription();

        //app.UseAuthorization();  In Application Layer, we have added the authorization policy

        app.MapAuthEndpoints();
        app.MapTaskEndpoints();

        Log.Information("Starting App...");
        app.Run();

        #endregion
    }

    #region Private utilities

    // this could be in AddInfrastructe(...)? i prefer here . . .
    private static async void SeedTasksData(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await SeedData.SeedTasksAsync(appDbContext);

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            await SeedData.SeedAdminUsersAsync(userManager, roleManager);
        }
    }
    #endregion

}
