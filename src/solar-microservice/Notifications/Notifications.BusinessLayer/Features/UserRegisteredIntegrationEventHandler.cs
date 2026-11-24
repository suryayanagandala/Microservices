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
        public class UserRegisteredIntegrationEventHandler(IUnitOfWork unitOfWork, ILogger<UserRegisteredIntegrationEventHandler> logger) : IIntegrationEventHandler<UserRegisteredIntegrationEvent>
        {
            public async Task HandleAsync(UserRegisteredIntegrationEvent message, CancellationToken cancellationToken = default)
            {
                var notification = new Notification
                {
                    UserId = message.userId,
                    Title = "Welcome",
                    Message = $"Welcome {message.Name}!",
                    Type = NotificationType.Register.ToString(),
                    IsRead = false,
                    CreatedAt = DateTimeOffset.UtcNow
                };

                await unitOfWork.NotificationRepository.AddNotification(notification);
                logger.LogInformation("\"===== USER REGISTERED HANDLER =====\"");
                await unitOfWork.SaveChanges(cancellationToken);
            
            }
        }
    }
