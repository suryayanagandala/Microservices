using BuildingBlocks.Authentication.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Authentication_.Jwt
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _options;

        public JwtTokenGenerator(IOptions<JwtOptions> options)
        {
            _options=options.Value;
        }
        public string GenerateAccessToken(JwtUser jwtUser)
        {
            //var claims = new[]
            //{
            //    new Claim("UserId",userId),

            //    new Claim(ClaimTypes.NameIdentifier,userId),

            //    new Claim(ClaimTypes.Email,email),

            //    new Claim(ClaimTypes.Role,role)
            //};

            var claims = new List<Claim>
            {
                new(JwtClaimNames.UserId,jwtUser.UserId),
                new(JwtClaimNames.Role,jwtUser.Role),
                new(JwtClaimNames.Email,jwtUser.Email),
                new(JwtClaimNames.JwtId,Guid.NewGuid().ToString())
            };


            //convert secret key from json to bites

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

            //how the token will be signed
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //create the token with issuer,auience,claim
            var token =
            new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_options.ExpiryMinutes),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var bytes = new byte[64];

            using var random = RandomNumberGenerator.Create();

            random.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
