using BuildingBloacks.Behaviour;
using BuildingBlocks.Authentication_.Jwt;
using BuildingBlocks.Messaging.DependencyInjection;
using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Messaging.Topology;
using BuildingBlocks.Utility;
using Cart.ApplicationLayer.Interfaces;
using Cart.BusinessLayer.Consumers;
using Cart.BusinessLayer.Features.AddToCart;
using Cart.Infrastructure;
using Cart.Infrastructure.Persistance;
using Cart.Infrastructure.Repositories;
using Cart.Infrastructure.Respositories;
using CART.API.Hubs;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//register pstgre
builder.Services.AddDbContext<CartDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database"),
        b =>
        {
            b.MigrationsAssembly(typeof(CartDbContext).Assembly.FullName);
            b.CommandTimeout(1000);
        });
});


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICartRepository, CartRepository>();

builder.Services.AddScoped<ICartNotificationService, CartNotificationService>();

//register validations and behaviours
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddBehaviours();

builder.Services.AddCarter();

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblyContaining<AddToCartHandler>();   
});

builder.Services.AddValidatorsFromAssemblyContaining<AddToCartValidator>();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();


//for rabbit publish
builder.Services.AddMessaging(builder.Configuration);

//for httclient services registration
var catalogUrl = builder.Configuration["Services:Catalog"];

builder.Services.AddHttpClient<IProductService, ProductService>(client =>
{
  client.BaseAddress=new Uri(catalogUrl?? "http://localhost:7003/products/");
});

builder.Services.AddIntegrationEvent<
    OrderPaidIntegrationEvent,
    OrderPaidIntegrationEventHandler>
(
    queueName: QueueNames.Cart,
    routingKey: RoutingKeys.Orders.OrderPaid
);


builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await ApplyMigrationsAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCarter();
app.UseExceptionHandler(option => { });
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapHub<CartHub>("/carthub");
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

            var dbContext = scope.ServiceProvider
                .GetRequiredService<CartDbContext>();

            await dbContext.Database.MigrateAsync(cancellationToken);

            return;
        }
        catch (Exception exception) when (attempt < maxAttempts)
        {
            var logger = app.Services
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger("CartDatabaseMigration");

            var delay = TimeSpan.FromSeconds(Math.Min(attempt, 5));

            logger.LogWarning(
                exception,
                "Unable to apply Cart migrations. Retrying in {DelaySeconds} seconds ({Attempt}/{MaxAttempts}).",
                delay.TotalSeconds,
                attempt,
                maxAttempts);

            await Task.Delay(delay, cancellationToken);
        }
    }
}

