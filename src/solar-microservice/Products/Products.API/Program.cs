using BuildingBlocks.Observability.DependencyInjection;
using BuildingBloacks.Behaviour;
using BuildingBlocks.Authentication_.Jwt;
using BuildingBlocks.Contracts.Events;
using BuildingBlocks.Exceptions.Exceptions;
using BuildingBlocks.Utility;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Products.API.Extensions;
using Products.ApplicationLayer.Interfaces;
using Products.BusinessLayer.Features.CreateCategory;
using Products.CoreModels;
using Products.DbAccessLayer.Repositories;
using Serilog;
using System;
using BuildingBlocks.Messaging.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSolarObservability(builder.Configuration, "products-api");

//register pstgre
builder.Services.AddDbContext<ProductDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        b =>
        {
            b.MigrationsAssembly(typeof(ProductDbContext).Assembly.FullName);
            b.CommandTimeout(1000);
        });
});

//services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICategoryInterface, CategoryRepository>();
builder.Services.AddScoped<IProductInterface, ProductRepository>();
builder.Services.AddScoped<IUtilityInterface, UtlityReository>();

//register validations and behaviours
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddBehaviours();


//add carter
builder.Services.AddCarter();


//add mediatr
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblyContaining<CreateCategoryCommandHandler>();
});

//register health checks and swagger
builder.Services.SwagerConfiguration().AddHealthConfig(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryValidators>();

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


//for rabbit publish
builder.Services.AddMessaging(builder.Configuration);



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapCarter();

app.MapHealthMiddleware();
app.UseSerilogRequestLogging();
app.UseStaticFiles();
app.UseExceptionHandler(option => { });

app.UseAuthentication();
app.UseAuthorization();
app.Run();
