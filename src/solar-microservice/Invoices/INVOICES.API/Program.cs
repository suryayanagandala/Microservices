using BuildingBlocks.Authentication_.Jwt;
using BuildingBlocks.Messaging.DependencyInjection;
using BuildingBlocks.Utility;
using Invoices.Infrastructure;
using INVOICES.API.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InvoicesDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Database"),
        npgsql =>
        {
            npgsql.MigrationsAssembly(typeof(InvoicesDbContext).Assembly.FullName);
            npgsql.CommandTimeout(1000);
        });
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddInvoiceSwagger();
builder.Services.AddInvoiceHealthChecks(builder.Configuration);

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

app.MapInvoiceHealthChecks();

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
            var dbContext = scope.ServiceProvider.GetRequiredService<InvoicesDbContext>();

            await dbContext.Database.MigrateAsync(cancellationToken);
            return;
        }
        catch (Exception exception) when (attempt < maxAttempts)
        {
            var logger = app.Services
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("InvoicesDatabaseMigration");
            var delay = TimeSpan.FromSeconds(Math.Min(attempt, 5));

            logger.LogWarning(
                exception,
                "Unable to apply Invoices migrations. Retrying in {DelaySeconds} seconds ({Attempt}/{MaxAttempts}).",
                delay.TotalSeconds,
                attempt,
                maxAttempts);

            await Task.Delay(delay, cancellationToken);
        }
    }
}
