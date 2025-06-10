using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NativoChallenge.Application.Common;
using System.Reflection;

namespace NativoChallenge.Application.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        Assembly executingAssembly = Assembly.GetExecutingAssembly();

        // CQRS con MediatR
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(executingAssembly));

        // FluentValidation
        services.AddValidatorsFromAssembly(executingAssembly);

        // Validation pipeline with FluentValidation + MediatR
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // Automapper
        services.AddAutoMapper(executingAssembly);

        return services;
    }
}
