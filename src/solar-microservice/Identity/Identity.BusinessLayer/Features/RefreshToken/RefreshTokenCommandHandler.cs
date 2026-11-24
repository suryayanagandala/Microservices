using BuildingBlocks.Authentication.Jwt;
using BuildingBlocks.Authentication_.Jwt;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Identity.Application.Interfaces;
using Identity.BusinessLayer.Features.LoginUser;
using Identity.Domain.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Identity.BusinessLayer.Features.RefreshToken
{
    internal class RefreshTokenCommandHandler(IUnitOfWork unitOfWork,IJwtTokenGenerator jwtTokenGenerator,IOptions<JwtOptions> options) 
        : ICommandHandler<RefreshTokenRequest, LoginUserResponse>
    {
        public async  Task<LoginUserResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var existingToken = await unitOfWork.refreshTokenInterface.GetTokenAsync(request.refreshToken);

            if (existingToken is null)
            {
                throw new UnauthorizedException("Invalid refresh token.");
            }

            if (existingToken.IsRevoked)
            {
                throw new UnauthorizedException("Refresh token has been revoked.");
            }

            if (existingToken.ExpiryDate <= DateTimeOffset.Now)
            {
                throw new UnauthorizedException("Refresh token has expired.");
            }

            var user = await unitOfWork.userInterface.GetUserById(existingToken.UserId);

            if (user is null)
            {
                throw new UnauthorizedException("User not found.");
            }

            var jwtUser = new JwtUser
            {
                UserId = user.UserId,
                Email = user.Email,
                Role = user.Role.RoleName
            };

            var accessToken =jwtTokenGenerator.GenerateAccessToken(jwtUser);

            var refreshToken =jwtTokenGenerator.GenerateRefreshToken();

            existingToken.IsRevoked = true;

            existingToken.RevokedAt = DateTimeOffset.Now;

            existingToken.ReplacedByToken = refreshToken;

            var newToken = new RefreshTokens
            {
                UserId = user.UserId,

                RefreshToken = refreshToken,

                CreatedAt = DateTimeOffset.Now,

                ExpiryDate =DateTimeOffset.Now
                           .AddDays(options.Value.RefreshTokenExpiryDays),

                IsRevoked = false
            };

            await unitOfWork.refreshTokenInterface.AddToken(newToken);

            await unitOfWork.SaveChanges(cancellationToken);

            return new LoginUserResponse(user.UserId, user.Email, user.Role.RoleName, 
                accessToken, refreshToken);
        }
    }
}
