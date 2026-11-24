using BuildingBlocks.CQRS;
using Identity.Application.Interfaces;
using Identity.CoreModels.Entities;
using Identity.CoreModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.ForGotPassword
{
    internal class ForgotPasswordCommandHandler(IUnitOfWork unitOfWork,IUtilityInterface utilityInterface) : ICommandHandler<ForgotPasswordReq, ForgotPasswordResponse>
    {
        public async Task<ForgotPasswordResponse> Handle(ForgotPasswordReq request, CancellationToken cancellationToken)
        {
            var existUser = await unitOfWork.userInterface.GetUserByEmail(request.email);
            if (existUser is null) throw new BuildingBlocks.Exceptions.Exceptions.NotFoundException("User not found");

            var generateOtp =await utilityInterface.GenerateRandomOtpText();

            var currentTime = DateTimeOffset.Now;
            var otpManagerDto = new OtpManager
            {
                UserId = existUser.UserId,
                EmailId = request.email,
                IsUsed = false,
                CreatdAt = currentTime,
                ExpiredTime = currentTime.AddMinutes(5),
                OtpType = OtpTypes.ForgotPassword.ToString(),
                OtpText = generateOtp
            };

            await unitOfWork.userInterface.AddOtpManager(otpManagerDto);
            await unitOfWork.SaveChanges(cancellationToken);

            return new ForgotPasswordResponse(otpManagerDto.OtpText);
        }
    }
}
