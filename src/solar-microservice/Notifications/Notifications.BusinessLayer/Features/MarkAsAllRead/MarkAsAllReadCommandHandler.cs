using BuildingBlocks.Authentication_.Abstractions;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Notifications.ApplicationLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.BusinessLayer.Features.MarkAsAllRead
{
    public class MarkAsAllReadCommandHandler(IUnitOfWork unitOfWork,ICurrentUser currentUser) : IQueryHandler<MarkAsAllReadRequest, MarkAsAllReadResponse>
    {
        public async Task<MarkAsAllReadResponse> Handle(MarkAsAllReadRequest request, CancellationToken cancellationToken)
        {
            var userId=currentUser.UserId;
            if (userId is null) throw new BadRequestException("Invalid User");

            var msgs = await unitOfWork.NotificationRepository.GetUserNotifications(userId);
            foreach (var msg in msgs)
            {
                msg.IsRead= true;

            }

            await unitOfWork.SaveChanges(cancellationToken);

            return new MarkAsAllReadResponse(msgs);
        }
    }
}
