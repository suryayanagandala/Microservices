using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Products.API.Extensions
{
    public static class HealthCheckExtension
    {
        public static IServiceCollection AddHealthConfig(this IServiceCollection services, IConfiguration builder)
        {
            services.AddHealthChecks().AddNpgSql(
                builder.GetConnectionString("DefaultConnection")!,
                name: "PostgreSql",
                failureStatus: HealthStatus.Unhealthy).AddRabbitMQ(
                name: "RabbitMQ", failureStatus: HealthStatus.Unhealthy);
            return services;
            
        }

        public static IEndpointRouteBuilder MapHealthMiddleware(this IEndpointRouteBuilder app)
        {
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse
            });
            return app;
        }
    }
}
