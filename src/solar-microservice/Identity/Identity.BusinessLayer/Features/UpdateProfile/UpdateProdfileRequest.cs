using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.UpdateProfile
{
   public record UpdateProfileRequest(string userId,string userName,string phoneNumber,string address,string email):ICommand<UpdateProfileResponse>;
}
