namespace PAYMENTS.API.Extensions;

public static class SwaggerService
{
    public static IServiceCollection AddPaymentSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
            {
                Title = "Payments API",
                Version = "v1",
                Description = "Solar Commerce Payments Microservice"
            });
        });

        return services;
    }
}
