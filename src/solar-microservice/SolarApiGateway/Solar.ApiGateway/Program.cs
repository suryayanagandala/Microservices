using BuildingBlocks.Observability.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSolarObservability(builder.Configuration, "solar-api-gateway");

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "http://localhost:4600",
            "http://localhost:56854")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors("AngularClient");

app.MapGet("/", () => Results.Ok(new
{
    service = "Solar API Gateway",
    status = "Running"
}));

app.MapReverseProxy();

app.Run();
