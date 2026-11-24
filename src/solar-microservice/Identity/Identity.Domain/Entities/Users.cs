using Identity.CoreModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entities
{
    public class Users
    {
        public string UserId { get; set; } = default!;

        public string UserName { get; set; } = string.Empty;


        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset UpdatedAt { get; set; }

        public string PhoneNumber {  get; set; } = string.Empty;

        public int RoleId { get; set; }

        public string Address { get; set; } = default!;

        public string CreatedBy { get; set; } = "Admin";

        public Roles Role { get; set; } = default!;




        public ICollection<RefreshTokens> RefreshTokens { get; set; } = new List<RefreshTokens>();

        public ICollection<OtpManager> OtpManagers { get; set; }=new List<OtpManager>();
    }
}
