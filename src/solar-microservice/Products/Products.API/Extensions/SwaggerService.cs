namespace Products.API.Extensions
{
    public static  class SwaggerService
    {
        public static IServiceCollection SwagerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
                {
                    Title = "Catalog API",
                    Version = "v1",
                    Description = "Solar Commerce Catalog Microservice"
                });
            });

            return services;
        }
    }
}
