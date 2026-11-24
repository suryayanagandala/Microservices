using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PAYMENTS.API.Extensions;

public static class HealthCheckExtension
{
    public static IServiceCollection AddPaymentHealthChecks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddNpgSql(
                configuration.GetConnectionString("Database")!,
                name: "PostgreSQL",
                failureStatus: HealthStatus.Unhealthy)
            .AddRabbitMQ(
                name: "RabbitMQ",
                failureStatus: HealthStatus.Unhealthy);

        return services;
    }

    public static IEndpointRouteBuilder MapPaymentHealthChecks(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return endpoints;
    }
}
