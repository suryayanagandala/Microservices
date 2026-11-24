using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBloacks.Behaviour
{
    public static  class DependencyInjection
    {
        public static IServiceCollection AddBehaviours(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavour<,>));

            return services;
        }
    }
}
