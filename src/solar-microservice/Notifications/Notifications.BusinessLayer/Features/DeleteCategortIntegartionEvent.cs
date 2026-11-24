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

namespace Notifications.BusinessLayer.Features
{
    //[RabbitConsumer(QueueNames.Notification, RoutingKeys.DeleteCategory)]
    public class DeleteCategortIntegartionEvent(IUnitOfWork unitOfWork,ILogger<DeleteCategortIntegartionEvent> logger) : IIntegrationEventHandler<DeleteCategoryEvent>
    {
        public async Task HandleAsync(DeleteCategoryEvent message, CancellationToken cancellationToken = default)
        {
            var notification = new Notification
            {
                UserId = message.userId,
                Title = "DeleteCategory",
                Message = $"{message.message}!",
                Type = NotificationType.Category.ToString(),
                IsRead = false,
                CreatedAt = DateTimeOffset.UtcNow
            };

            await unitOfWork.NotificationRepository.AddNotification(notification);
            await unitOfWork.SaveChanges(cancellationToken);
            logger.LogInformation("===== DELETE CATEGORY HANDLER =====");
        }
    }
}
