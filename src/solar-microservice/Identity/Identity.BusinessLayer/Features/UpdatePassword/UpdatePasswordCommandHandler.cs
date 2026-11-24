using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Identity.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.UpdatePassword
{
    internal class UpdatePasswordCommandHandler(IUnitOfWork unitOfWork,IUtilityInterface utilityInterface) : ICommandHandler<UpdatePasswordRequest, UpdatePasswordResponse>
    {
        public async Task<UpdatePasswordResponse> Handle(UpdatePasswordRequest request, CancellationToken cancellationToken)
        {
            var existUser = await unitOfWork.userInterface.GetUserByEmail(request.email);
            if (existUser is null)
                throw new BadRequestException("User not found");

            var oldpassword = request.oldPassword.Trim();
            var newPassword=request.newPassword.Trim();

            var verifyPassword = await utilityInterface.VerifyPassword(oldpassword, existUser.PasswordHash);
            if (!verifyPassword)
                throw new BadRequestException("Incorrect old password");
            var setnewPasswordValidation=await utilityInterface.VerifyPassword(newPassword, existUser.PasswordHash);
            if (setnewPasswordValidation)
                throw new BadRequestException("New password must be different ");

            string setNewPassword = await utilityInterface.PasswordHash(newPassword);

            existUser.PasswordHash = setNewPassword;
            existUser.UpdatedAt = DateTimeOffset.Now;

            await unitOfWork.SaveChanges(cancellationToken);
            return new UpdatePasswordResponse(true);
            
        }

    }
}
