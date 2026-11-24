using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.CreateUser
{
    public record RegisterCommand(string password, string email, string userName,string phoneNumber,string address):ICommand<CreateUserResponse>;
}
