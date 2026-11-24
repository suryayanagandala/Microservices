using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Interfaces
{
    public interface IRefreshTokenInterface
    {
        Task AddToken(RefreshTokens refreshTokens);

        Task<RefreshTokens?> GetTokenAsync(string token);

        Task RevokeAsync(RefreshTokens token);
    }
}
