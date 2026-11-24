using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.VerifyOtp
{
    public record VerifyOtpRequest(string email,string otptext):ICommand<VerifyOtpResponse>;
}
