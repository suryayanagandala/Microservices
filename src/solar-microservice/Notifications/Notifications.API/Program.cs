using BuildingBlocks.Observability.DependencyInjection;

using BuildingBlocks.Authentication_.Jwt;
using Microsoft.EntityFrameworkCore;
using Notifications.ApplicationLayer.Interfaces;
using Notifications.CoreModels.Entities;
using Notifications.DbAccessLayer.Infrastructure;
using BuildingBlocks.Messaging.DependencyInjection;
using BuildingBlocks.Messaging.Topology;
using BuildingBlocks.Contracts.Events;
using Notifications.BusinessLayer.Features;
using Notifications.BusinessLayer.Features.ConsumerEvents;
using BuildingBloacks.Behaviour;
using Notifications.BusinessLayer.Features.GetNotifications;
using BuildingBlocks.Utility;
using Notifications.BusinessLayer.Features.MarkAsAllRead;
using FluentValidation;
using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSolarObservability(builder.Configuration, "notifications-api");

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

//db connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b =>
        {
            b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            b.CommandTimeout(1000);
        });
});

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();



builder.Services.AddMessaging(builder.Configuration);

builder.Services.AddIntegrationEvent<UserRegisteredIntegrationEvent, UserRegisteredIntegrationEventHandler>(QueueNames.Notification, RoutingKeys.UserRegistered);

builder.Services.AddIntegrationEvent<DeleteCategoryEvent, DeleteCategortIntegartionEvent>(QueueNames.Notification, RoutingKeys.CategoryDeleted);

builder.Services.AddIntegrationEvent<CreateProductEvent, CreateProdcutEventHandler>(QueueNames.Notification, RoutingKeys.ProductCreated);

builder.Services.AddIntegrationEvent<UpdateProductEvent, UpdateProductEventHandler>(QueueNames.Notification, RoutingKeys.ProductUpdated);

builder.Services.AddIntegrationEvent<DeleteProductEvent, DeleteProductEventHandler>(QueueNames.Notification, RoutingKeys.ProductDeleted);

builder.Services.AddIntegrationEvent<UploadProductImageEvent, UploadProductImageEventHandler>(QueueNames.Notification, RoutingKeys.ImageCreated);

builder.Services.AddIntegrationEvent<UpdateProductImageEvent, UpdateProductImageEventHandler>(QueueNames.Notification, RoutingKeys.ImageUpdated);

builder.Services.AddIntegrationEvent<DeleteProductImageEvent,DeleteProductImageEventHandler>(QueueNames.Notification,RoutingKeys.ImageDeleted);

builder.Services.AddCarter();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<GetNotificationsCommandHandler>();
});

builder.Services.AddValidatorsFromAssemblyContaining<MarkAsAllReadValidator>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

//register pipelines by the use of using statement
builder.Services.AddBehaviours();



var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();
app.UseExceptionHandler(option => { });


app.Run();
