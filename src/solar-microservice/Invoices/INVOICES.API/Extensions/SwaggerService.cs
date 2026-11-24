namespace INVOICES.API.Extensions;

public static class SwaggerService
{
    public static IServiceCollection AddInvoiceSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
            {
                Title = "Invoices API",
                Version = "v1",
                Description = "Solar Commerce Invoices Microservice"
            });
        });

        return services;
    }
}
