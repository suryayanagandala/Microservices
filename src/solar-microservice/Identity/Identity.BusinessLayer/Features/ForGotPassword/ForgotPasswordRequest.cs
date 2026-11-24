using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.ForGotPassword
{
   public record ForgotPasswordReq(string email):ICommand<ForgotPasswordResponse>;
}
