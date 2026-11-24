using Identity.CoreModels.Dto_s;
using Identity.CoreModels.Entities;
using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Interfaces
{
    public interface IUserInterface
    {
        Task<Users?> GetUserById(string userId);

        Task<Users?> GetUserByEmail(string email);

        Task<bool> AddUser(Users users);

        Task UpdateUser(string userId,Users users);

        Task<bool> EmailExists(string email);


        Task UpdateUserProfile(UpdateUserProfile updateProfile);

        Task AddOtpManager(OtpManager otp);

        Task<OtpManager?> VerifyOtp(string email, string otptext);

        Task<OtpManager?> ResetPassword(Guid id);
    }
}
