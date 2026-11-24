using BuildingBlocks.Contracts.Events;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using BuildingBlocks.Messaging.Contracts;
using BuildingBlocks.Messaging.Topology;
using BuildingBlocks.Utility;
using Identity.Application.Interfaces;
using Identity.CoreModels.Enums;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.CreateUser
{
    public class CreateUserCommandHandler(IUnitOfWork unitOfWork,IEventBus eventbus) : ICommandHandler<RegisterCommand, CreateUserResponse>
    {
        public async Task<CreateUserResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var exists = await unitOfWork.userInterface.EmailExists(request.email);

            if (exists)
            {
                 throw new AlreadyExists("EmailId is exists");
            }

            var hashPassword = PasswordHash.HashPassword(request.password.Trim());

            var idgen = new GenerateId(unitOfWork);
            var user = new Users
            {
                UserId = await idgen.GenerateUserId(),
                PasswordHash= hashPassword,
                UserName = request.userName.Trim().ToLower(),
                Email = request.email.Trim().ToLower(),
                CreatedAt = DateTimeOffset.Now,
                IsActive = true,
                PhoneNumber= request.phoneNumber,
                RoleId= ((int)RolesEnum.User),
                Address=request.address,
            };

            await unitOfWork.userInterface.AddUser(user);
            await unitOfWork.SaveChanges(cancellationToken);

            await eventbus.PublishAsync<UserRegisteredIntegrationEvent>(new UserRegisteredIntegrationEvent(user.UserId, user.UserName, user.Email
                ), RoutingKeys.UserRegistered, cancellationToken);

            
            return new CreateUserResponse(user.UserId);
        }
    }
}
