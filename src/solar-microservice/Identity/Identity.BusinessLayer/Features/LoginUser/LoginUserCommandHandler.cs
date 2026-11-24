using BuildingBlocks.Authentication.Jwt;
using BuildingBlocks.Authentication_.Jwt;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.LoginUser
{
    public class LoginUserCommandHandler(IUnitOfWork unitOfWork, IJwtTokenGenerator jwtTokenGenerator
        ,IUtilityInterface utilityInterface,IOptions<JwtOptions> options) : ICommandHandler<LoginUserRequest, LoginUserResponse>
    {
        public async Task<LoginUserResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.userInterface.GetUserByEmail(request.Email);

            if (user is null)
                throw new NotFoundException("Invalid EmailId");
            var isPasswordCorrect = await utilityInterface.VerifyPassword(request.Password, user.PasswordHash);

            if (!isPasswordCorrect)
                throw new NotFoundException("Invalid Password");
            var jwtUser = new JwtUser
            {
                Email = user.Email,
                UserId=user.UserId,
                Role=user.Role.RoleName,

            };

            var accessToken=jwtTokenGenerator.GenerateAccessToken(jwtUser);

            var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

            var refreshTokenDto = new RefreshTokens
            {
                UserId = user.UserId,
                CreatedAt = DateTimeOffset.Now,
                ExpiryDate = DateTimeOffset.Now.AddDays(options.Value.RefreshTokenExpiryDays),
                RefreshToken= refreshToken,
                IsRevoked=false
            };

            await unitOfWork.refreshTokenInterface.AddToken(refreshTokenDto);

            await unitOfWork.SaveChanges(cancellationToken);

            return new LoginUserResponse
            (
                user.UserId,user.Email,user.Role.RoleName,accessToken,refreshToken
            );
        }
    }
}
