using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.BusinessLayer.Features.GetNotifications
{
    public record GetNotificationsRequest(string? userId):IQuery<GetNotificationsResponse>;
}
