using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Migration.Application.Engine;
using Migration.Application.Interfaces;
using Migration.Application.Service;
using Migration.Busniesslayer.Service;
using Migration.Infrastructure.DbSetUp;
using Migration.Infrastructure.Respositories;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        
        // SQL SERVER DB CONTEXT
       
        services.AddDbContext<SqlServerDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

        // POSTGRES DB CONTEXT
       
        services.AddDbContext<PostgresDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres")));

       
        // SERVICES
       
        //services.AddScoped<ProductEngine>();

        services.AddScoped(typeof(ISqlRepository<>), typeof(SqlRepository<>));
        services.AddScoped(typeof(IPostgresRepository<>), typeof(PostgreRepository<>));

        //services.AddScoped<ProductImageMigrationSrvice>();
        services.AddScoped<ProductsMigrationsService>();
       
    })
    .Build();

using var scope = host.Services.CreateScope();

//var productService = scope.ServiceProvider.GetRequiredService<ProductsMigrationsService>();

var productimageservice=scope.ServiceProvider.GetRequiredService<ProductImageMigrationSrvice>();


Console.WriteLine(" Category Migration Started...");

//await productService.MigrateAsync();

await productimageservice.MigrateAsync();


Console.WriteLine("Category Migration Completed!");