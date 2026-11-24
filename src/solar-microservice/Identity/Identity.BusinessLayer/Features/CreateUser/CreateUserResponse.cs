using Identity.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.CreateUser
{
   public record CreateUserResponse(string userId);


    public  class GenerateId(IUnitOfWork unitOfWork)
    {
        public  async Task<string> GenerateUserId()
        {
            while (true)
            {
                string  id = $"{Random.Shared.Next(100000,999999)}";

                var user=await unitOfWork.userInterface.GetUserById(id);

                if(user is null)
                {
                    return id;
                }

            }
        }
    }
}
