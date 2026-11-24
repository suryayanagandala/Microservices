using BuildingBloacks.Behaviour;
using BuildingBlocks.Authentication_.Jwt;
using BuildingBlocks.Messaging.DependencyInjection;
using BuildingBlocks.Utility;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Payments.ApplicationLayer.Interfaces;
using Payments.BusinessLayer.Features.CreatePayment;
using Payments.Infrastructure;
using Payments.Infrastructure.Repositories;
using PAYMENTS.API.Extensions;
using Serilog;
using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PaymentsDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Database"),
        npgsql =>
        {
            npgsql.MigrationsAssembly(typeof(PaymentsDbContext).Assembly.FullName);
            npgsql.CommandTimeout(1000);
        });
});

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblyContaining(typeof(CreatePaymentHandler));
});

builder.Services.AddValidatorsFromAssemblyContaining(typeof(CreatePaymentValidator));

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddPaymentSwagger();
builder.Services.AddPaymentHealthChecks(builder.Configuration);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day);
});

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddMessaging(builder.Configuration);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IPaymentRepository, PaymentRespository>();

//register pipelines by the use of using statement
builder.Services.AddBehaviours();

builder.Services.AddCarter();

var catalogUrl = builder.Configuration["Services:Orders"];

builder.Services.AddHttpClient<IOrderService,OrderService>(client =>
{
    client.BaseAddress = new Uri(catalogUrl ?? "http://localhost:7005");
});

var app = builder.Build();

await ApplyMigrationsAsync(app);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseExceptionHandler(_ => { });
app.UseAuthentication();
app.UseAuthorization();

app.MapPaymentHealthChecks();

app.MapCarter();

app.Run();

static async Task ApplyMigrationsAsync(WebApplication app)
{
    const int maxAttempts = 10;
    var cancellationToken = app.Lifetime.ApplicationStopping;

    for (var attempt = 1; attempt <= maxAttempts; attempt++)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentsDbContext>();

            await dbContext.Database.MigrateAsync(cancellationToken);
            return;
        }
        catch (Exception exception) when (attempt < maxAttempts)
        {
            var logger = app.Services
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("PaymentsDatabaseMigration");
            var delay = TimeSpan.FromSeconds(Math.Min(attempt, 5));

            logger.LogWarning(
                exception,
                "Unable to apply Payments migrations. Retrying in {DelaySeconds} seconds ({Attempt}/{MaxAttempts}).",
                delay.TotalSeconds,
                attempt,
                maxAttempts);

            await Task.Delay(delay, cancellationToken);
        }
    }
}
