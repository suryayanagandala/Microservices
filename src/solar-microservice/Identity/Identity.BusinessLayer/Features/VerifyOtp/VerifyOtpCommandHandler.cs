using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Identity.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.BusinessLayer.Features.VerifyOtp
{
    public class VerifyOtpCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<VerifyOtpRequest, VerifyOtpResponse>
    {
        public async Task<VerifyOtpResponse> Handle(VerifyOtpRequest request, CancellationToken cancellationToken)
        {
            var otpData = await unitOfWork.userInterface.VerifyOtp(request.email, request.otptext);
            if(otpData is null)
            {
                throw new BadRequestException("Invalid otp or invalid email");
            }
            //verify otp
            if (otpData.ExpiredTime < DateTimeOffset.Now)
            {
                throw new BadRequestException("Time is expired");
            }

            if (otpData.IsUsed)
            {
                throw new BadRequestException("OTP already used");
            }


            var token = Guid.NewGuid();

            otpData.Token = token;
            otpData.IsUsed = true;
            await unitOfWork.SaveChanges(cancellationToken);

            return new VerifyOtpResponse(otpData.Token);
        }
    }
}
