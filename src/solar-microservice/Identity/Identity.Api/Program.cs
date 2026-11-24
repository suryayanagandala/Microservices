using BuildingBlocks.Observability.DependencyInjection;
using BuildingBloacks.Behaviour;
using BuildingBlocks.Authentication_.Jwt;
using BuildingBlocks.Messaging.DependencyInjection;
using BuildingBlocks.Utility;
using Carter;
using FluentValidation;
using Identity.Application.Interfaces;
using Identity.BusinessLayer.Features.AddRole;
using Identity.BusinessLayer.Features.CreateUser;
using Identity.CoreModels.Persistance;
using Identity.DbAccessLayer.Infrastructure;
using Identity.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSolarObservability(builder.Configuration, "identity-api");


//register iservice collection service from building blocks
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);



builder.Services.AddAuthorization();


//add rabbit mq
builder.Services.AddMessaging(builder.Configuration);


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserInterface,UserRepository>();

builder.Services.AddScoped<IRoleInterface,RoleRepository>();

builder.Services.AddScoped<IRefreshTokenInterface,RefreshTokenRepository>();

builder.Services.AddTransient<IUtilityInterface, UtilityRepository>();


//register validators
builder.Services.AddValidatorsFromAssembly(typeof(CreateUserValidation).Assembly);

//add carter

builder.Services.AddCarter();


//Hnadler are in service 
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CreateUserCommandHandler>();
    cfg.RegisterServicesFromAssemblyContaining<RolesValidation>();
});


//handled jwt confugure in appseting jewt through jwtoprion class we have to use thers app directly
//builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));


builder.Services.AddExceptionHandler<CustomExceptionHandler>();

//register pipelines by the use of using statement
builder.Services.AddBehaviours();

var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler(option => { });

app.UseAuthentication();
app.UseAuthorization();
app.Run();
