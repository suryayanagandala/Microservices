using Identity.Application.Interfaces;
using Identity.CoreModels.Dto_s;
using Identity.CoreModels.Entities;
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
    public class UserRepository : IUserInterface
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddOtpManager(OtpManager otp)
        {
            await _db.OtpManagers.AddAsync(otp);
        }

        public async Task<bool> AddUser(Users users)
        {
            await _db.Users.AddAsync(users);
            return true;
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _db.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<Users?> GetUserByEmail(string email)
        {
           return await _db.Users.Include(x=>x.Role).FirstOrDefaultAsync(x=>x.Email==email&&x.IsActive);
        }

        public async Task<Users?> GetUserById(string userId)
        {
            return await _db.Users.Include(x=>x.Role).FirstOrDefaultAsync(x=>x.UserId==userId&&x.IsActive); 
        }

        public async Task<OtpManager?> ResetPassword(Guid id)
        {
            return await _db.OtpManagers.FirstOrDefaultAsync(x => x.Token == id);
        }

        public async Task UpdateUser(string userId, Users users)
        {
            var existUser=await _db.Users.FirstOrDefaultAsync(    x=>x.UserId==userId);
            if (existUser!=null)
            {
                _db.Users.Update(users);
                await _db.SaveChangesAsync();
               
            }
           
        }

        public async Task UpdateUserProfile(UpdateUserProfile updateProfile)
        {
            var existUser = await _db.Users.FirstOrDefaultAsync(x => x.UserId == updateProfile.UserId);
            if(existUser is null)
            {
                return;
            }
            existUser.UserName = updateProfile.UserName;    
            existUser.Address = updateProfile.Address;
            existUser.Email = updateProfile.Email;
            existUser.PhoneNumber= updateProfile.PhoneNumber;

            _db.Users.Update(existUser);
            await _db.SaveChangesAsync();
        }

        public Task<OtpManager?> VerifyOtp(string email, string otptext)
        {
            return _db.OtpManagers.Where(x => x.EmailId == email && x.OtpText == otptext && !x.IsUsed)
                .OrderByDescending(x => x.CreatdAt).FirstOrDefaultAsync();
        }


    }
}
