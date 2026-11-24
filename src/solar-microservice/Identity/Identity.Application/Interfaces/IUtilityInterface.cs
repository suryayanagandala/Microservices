using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Interfaces
{
    public interface IUtilityInterface
    {
        Task<string> PasswordHash(string password);

        Task<bool> VerifyPassword(string password,string saltPassword);

        Task<string> GenerateRandomOtpText();
    }
}
