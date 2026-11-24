using BCrypt.Net;
using Identity.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Identity.DbAccessLayer.Infrastructure
{
    public class UtilityRepository : IUtilityInterface
    {
        public async Task<string> GenerateRandomOtpText()
        {
            //A cryptographic generator produces values where knowing previous values does not help predict the next one
            var randomNumber = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

            return await  Task.FromResult(randomNumber);
        }

        public  Task<string> PasswordHash(string password)
        {
            var hashPassword= BCrypt.Net.BCrypt.HashPassword(password);
            return Task.FromResult(hashPassword);
        }

        public  Task<bool> VerifyPassword(string password, string saltPassword)
        {
            var verifyPassword=BCrypt.Net.BCrypt.Verify(password, saltPassword);
            return Task.FromResult(verifyPassword);
        }
    }
}
