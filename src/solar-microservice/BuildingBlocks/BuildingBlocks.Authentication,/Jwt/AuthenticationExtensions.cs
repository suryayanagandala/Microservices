using BuildingBlocks.Authentication_.Abstractions;
using BuildingBlocks.Authentication_.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Authentication_.Jwt
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection
            AddJwtAuthentication(
                this IServiceCollection services,
                IConfiguration configuration)
        {
            services.Configure<JwtOptions>(
                configuration.GetSection(
                    JwtOptions.SectionName));

            var jwtOptions =
                configuration
                .GetSection(JwtOptions.SectionName)
                .Get<JwtOptions>()!;

            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.MapInboundClaims = false;
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateIssuerSigningKey = true,
                            ValidateLifetime = true,

                            ValidIssuer =
                                jwtOptions.Issuer,

                            ValidAudience =
                                jwtOptions.Audience,

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(
                                        jwtOptions.SecretKey)),

                                NameClaimType="sub",
                                RoleClaimType="role"
                        };
                });

            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUser,CurentUser>();

            services.AddSingleton< IJwtTokenGenerator,
                JwtTokenGenerator>();

            return services;


        }
    }
}
