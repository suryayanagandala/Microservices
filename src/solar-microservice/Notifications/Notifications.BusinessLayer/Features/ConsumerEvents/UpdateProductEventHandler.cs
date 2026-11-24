using BuildingBlocks.Contracts.Events;
using BuildingBlocks.Messaging.Contracts;
using Microsoft.Extensions.Logging;
using Notifications.ApplicationLayer.Interfaces;
using Notifications.CoreModels.Entities;
using Notifications.CoreModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.BusinessLayer.Features.ConsumerEvents
{
    public class UpdateProductEventHandler(IUnitOfWork unitOfWork,ILogger<UpdateProductEventHandler> logger) : IIntegrationEventHandler<UpdateProductEvent>
    {
        public async Task HandleAsync(UpdateProductEvent message, CancellationToken cancellationToken = default)
        {

            var notification = new Notification
            {
                UserId = message.userId,
                Title = "Product Update",
                Message = $"{message.message}!",
                Type = NotificationType.Category.ToString(),
                IsRead = false,
                CreatedAt = DateTimeOffset.UtcNow
            };

            await unitOfWork.NotificationRepository.AddNotification(notification);
            await unitOfWork.SaveChanges(cancellationToken);
            logger.LogInformation("===== UPDATE PRODUCT HANDLER =====");
        }
    }
}
