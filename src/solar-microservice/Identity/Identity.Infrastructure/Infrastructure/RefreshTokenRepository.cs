using Identity.Application.Interfaces;
using Identity.CoreModels.Persistance;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.DbAccessLayer.Infrastructure
{
    public class RefreshTokenRepository : IRefreshTokenInterface
    {
        private readonly ApplicationDbContext _db;
        public RefreshTokenRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddToken(RefreshTokens refreshTokens)
        {
            await _db.RefreshTokens.AddAsync(refreshTokens);
        }

        public async Task<RefreshTokens?> GetTokenAsync(string token)
        {
            return await _db.RefreshTokens.FirstOrDefaultAsync(x=>x.RefreshToken == token);
        }

        public Task RevokeAsync(RefreshTokens token)
        {
            token.IsRevoked = true;
            _db.RefreshTokens.Update(token);    
            return Task.CompletedTask;
        }
    }
}
