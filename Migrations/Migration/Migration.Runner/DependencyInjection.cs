using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Migration.Infrastructure.DbSetUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Runner
{
    public static  class DependencyInjection
    {
        public static IServiceCollection AddMigrationServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<SqlServerDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            });

            services.AddDbContext<PostgresDbContext>(options => options.UseNpgsql(
                configuration.GetConnectionString("Postgres")));



            return services;
        }
    }
}
