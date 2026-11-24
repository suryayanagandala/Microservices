using Notifications.CoreModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.BusinessLayer.Features.GetNotifications
{
    public record GetNotificationsResponse(IReadOnlyList<Notification> notifications);
}
