using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Identity.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.ResetPassword
{
    internal class ResetPasswordCommandHandler(IUnitOfWork unitOfWork,IUtilityInterface utilityInterface) : ICommandHandler<ResetPasswordReq, ResetPasswordResponse>
    {
        public async Task<ResetPasswordResponse> Handle(ResetPasswordReq request, CancellationToken cancellationToken)
        {
            var existOtpManager = await unitOfWork.userInterface.ResetPassword(request.guid);
            if (existOtpManager is null)
                throw new BadRequestException("Invalid token");
            else if (existOtpManager.IsUsed)
            {
                throw new BadRequestException("Otp is already used");
            }
            else if (existOtpManager.ExpiredTime < DateTimeOffset.Now)
            {
                throw new BadRequestException("Token is already expired");
            }

            var existUser = await unitOfWork.userInterface.GetUserByEmail(existOtpManager.EmailId);
            if (existUser is null)
                throw new NotFoundException("Invalid user or user not found");

            existUser.UpdatedAt= DateTimeOffset.Now;

            var newPassword = request.password.Trim();
            var resetPassword = request.resetPassword.Trim();

            if (newPassword==resetPassword)
            {
                var password=await utilityInterface.PasswordHash(newPassword);
                existUser.PasswordHash= password;
                
            }
            else
            {
                throw new BadRequestException("Reset password must be same as new password");
            }

            await unitOfWork.SaveChanges(cancellationToken);

            return new ResetPasswordResponse(true);

        }
    }
}
