using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Notifications.ApplicationLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.BusinessLayer.Features.GetNotifications
{
    public class GetNotificationsCommandHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetNotificationsRequest, GetNotificationsResponse>
    {
        public async Task<GetNotificationsResponse> Handle(GetNotificationsRequest request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.userId))
            {
                var userExists = await unitOfWork.NotificationRepository.ExistUser(request.userId);

                if (!userExists)
                {
                    throw new NotFoundException("Invalid User");
                }
            }
            var response = await unitOfWork.NotificationRepository.GetAllNotifications(request.userId);

            return new GetNotificationsResponse(response);
        }
    }
}
