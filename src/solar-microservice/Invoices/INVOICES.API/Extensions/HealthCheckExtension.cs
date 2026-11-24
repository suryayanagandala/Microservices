using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace INVOICES.API.Extensions;

public static class HealthCheckExtension
{
    public static IServiceCollection AddInvoiceHealthChecks(
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

    public static IEndpointRouteBuilder MapInvoiceHealthChecks(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return endpoints;
    }
}
