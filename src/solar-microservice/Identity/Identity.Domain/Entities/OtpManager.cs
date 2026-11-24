using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Identity.CoreModels.Entities
{
    public class OtpManager
    {
        public int OtpId { get; set; }

        public string UserId { get; set; } = default!;

        public string OtpText { get; set; }= default!;

        public string OtpType { get; set; } = default!; 

        public bool IsUsed { get; set; }
        public string EmailId { get; set; } = default!; 

        public DateTimeOffset CreatdAt { get; set; }=DateTimeOffset.Now;

        public DateTimeOffset ExpiredTime { get; set; }

        public Guid Token {  get; set; } = default!;


    }
}
