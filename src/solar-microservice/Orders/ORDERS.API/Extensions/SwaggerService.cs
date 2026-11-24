namespace ORDERS.API.Extensions;

public static class SwaggerService
{
    public static IServiceCollection AddOrderSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
            {
                Title = "Orders API",
                Version = "v1",
                Description = "Solar Commerce Orders Microservice"
            });
        });

        return services;
    }
}
