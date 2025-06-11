namespace NativoChallenge.WebAPI.Common.Auth
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AppPolicies.OnlyAdmin, policy => policy.RequireRole(AppRoles.Admin));
                options.AddPolicy(AppPolicies.OnlyGuest, policy => policy.RequireRole(AppRoles.Guest));
            });
            return services;
        }
    }

}
