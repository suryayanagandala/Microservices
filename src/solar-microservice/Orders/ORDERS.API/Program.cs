using BuildingBloacks.Behaviour;
using BuildingBlocks.Authentication_.Jwt;
using BuildingBlocks.Exceptions.Exceptions;
using BuildingBlocks.Messaging.DependencyInjection;
using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Messaging.Topology;
using BuildingBlocks.Utility;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Orders.ApplicationLayer.Interfaces;
using Orders.BusinessLayer.BackGroundService;
using Orders.BusinessLayer.Consumers;
using Orders.BusinessLayer.Features.CreateOrder;
using Orders.CoreModels.Dto_s;
using Orders.Infrastructure;
using Orders.Infrastructure.Repositories;
using Orders.Infrastructure.Services;
using ORDERS.API.Extensions;
using ORDERS.API.Hubs;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrdersDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Database"),
        npgsql =>
        {
            npgsql.MigrationsAssembly(typeof(OrdersDbContext).Assembly.FullName);
            npgsql.CommandTimeout(1000);
        });
});

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IOrderNotificationService, OrderNotificationService>();

var productsUrl = builder.Configuration["Services:Products"];

builder.Services.AddHttpClient<IProductService, ProductService>(client =>
{
    client.BaseAddress = new Uri(productsUrl ?? "http://localhost:7003");
});

builder.Services.AddIntegrationEvent<CartCheckedOutIntegrationEvent,CartCheckedOutIntegrationEventHandler>(
        QueueNames.Order,RoutingKeys.Carts.CheckOut);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddBehaviours();
builder.Services.AddCarter();

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblyContaining<CreateOrderHandler>();
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderValidator>();

builder.Services.AddOrderSwagger();
builder.Services.AddOrderHealthChecks(builder.Configuration);

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
builder.Services.AddSignalR();

builder.Services.Configure<OrderCleanupOptions>(builder.Configuration.GetRequiredSection(OrderCleanupOptions.SectionName));

//background work
builder.Services.AddHostedService<OrderCleanupWorker>();


builder.Services.AddIntegrationEvent<PaymentCompletedIntegrationEvent, PaymentIntegrationEventHandler>(
        queueName: QueueNames.Order,
        routingKey: RoutingKeys.Payment.PaymentCreated);



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

app.MapCarter();
app.MapOrderHealthChecks();
app.MapHub<OrderHub>("/orderhub").RequireAuthorization();

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
            var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();

            await dbContext.Database.MigrateAsync(cancellationToken);
            return;
        }
        catch (Exception exception) when (attempt < maxAttempts)
        {
            var logger = app.Services
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("OrdersDatabaseMigration");
            var delay = TimeSpan.FromSeconds(Math.Min(attempt, 5));

            logger.LogWarning(
                exception,
                "Unable to apply Orders migrations. Retrying in {DelaySeconds} seconds ({Attempt}/{MaxAttempts}).",
                delay.TotalSeconds,
                attempt,
                maxAttempts);

            await Task.Delay(delay, cancellationToken);
        }
    }
}
