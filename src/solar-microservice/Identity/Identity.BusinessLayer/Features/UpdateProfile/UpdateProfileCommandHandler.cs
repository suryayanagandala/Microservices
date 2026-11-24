using BuildingBlocks.CQRS;
using Identity.Application.Interfaces;
using Minio.DataModel.Replication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity.CoreModels.Dto_s;

namespace Identity.BusinessLayer.Features.UpdateProfile
{
    public class UpdateProfileCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateProfileRequest, UpdateProfileResponse>
    {
        public async Task<UpdateProfileResponse> Handle(UpdateProfileRequest request, CancellationToken cancellationToken)
        {
            var existUser = await unitOfWork.userInterface.GetUserById(request.userId);

            if (existUser is null)
            {
                throw new BuildingBlocks.Exceptions.Exceptions.NotFoundException("UserId is not found");
            }
            var userDto = new UpdateUserProfile
            {
                UserId = request.userId.Trim().ToLower(),
                UserName=request.userName.Trim().ToLower(),
                Email=request.email.Trim().ToLower(),
                PhoneNumber=request.phoneNumber.Trim().ToLower(),
                Address=request.address.Trim().ToLower(),
            };

            await unitOfWork.userInterface.UpdateUserProfile(userDto);
            return new UpdateProfileResponse(true);
        }
    }
}
