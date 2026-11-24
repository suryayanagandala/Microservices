using BuildingBlocks.Authentication.Jwt;
using BuildingBlocks.Authentication_.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Authentication_.Service
{
    internal class CurentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurentUser(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }


        public string? UserId => this.httpContextAccessor.HttpContext?.User?.FindFirst(JwtClaimNames.UserId)?.Value;

        public string? Email => this.httpContextAccessor.HttpContext?.User?.FindFirst(JwtClaimNames.Email)?.Value;

        public string? Role => this.httpContextAccessor.HttpContext?.User?.FindFirst(JwtClaimNames.Role)?.Value;
    }
}
