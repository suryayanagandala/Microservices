using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.CoreModels.Dto_s
{
     public class UpdateUserProfile
    {
        public string UserName { get; set; } = default!;

        public string Address { get; set; } = default!;

        public string PhoneNumber { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string UserId { get; set; } = default!;
    }
}
