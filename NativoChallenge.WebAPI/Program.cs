
using Microsoft.OpenApi.Models;
using NativoChallenge.Infrastructure.Data.Configuration;
using NativoChallenge.Application.Configuration;
using NativoChallenge.Infrastructure.Data.EF;
using NativoChallenge.WebAPI.Endpoints;
using NativoChallenge.WebAPI.Extensions;

namespace NativoChallenge.WebAPI;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Services 

        // Application and Infrastructure services
        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddInfrastructure(builder.Configuration);

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
        });

        #endregion 

        var app = builder.Build();

        // Seed Task data
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

        //app.UseAuthorization();  Not really necessary, but we can improve auth and auth later...

        app.MapTaskEndpoints();

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
            await SeedData.SeedAsync(appDbContext);
        }
    }
    #endregion

}
