using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace BuildingBlocks.Observability.DependencyInjection;

public static class ObservabilityExtensions
{
    public static IServiceCollection AddSolarObservability(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(serviceName);

        var otlpEndpoint = configuration["OpenTelemetry:Otlp:Endpoint"]
            ?? configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];

        var telemetry = services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(
                serviceName: serviceName,
                serviceVersion: typeof(ObservabilityExtensions).Assembly.GetName().Version?.ToString() ?? "1.0.0"))
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation())
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation());

        if (Uri.TryCreate(otlpEndpoint, UriKind.Absolute, out var endpoint))
        {
            var protocolName = configuration["OpenTelemetry:Otlp:Protocol"]
                ?? configuration["OTEL_EXPORTER_OTLP_PROTOCOL"];
            var protocol = string.Equals(protocolName, "http/protobuf", StringComparison.OrdinalIgnoreCase)
                ? OtlpExportProtocol.HttpProtobuf
                : OtlpExportProtocol.Grpc;

            telemetry.UseOtlpExporter(protocol, endpoint);
        }

        return services;
    }
}