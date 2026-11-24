using BuildingBlocks.CQRS;
using Notifications.CoreModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.BusinessLayer.Features.MarkAsAllRead
{
    public record MarkAsAllReadResponse(IEnumerable<Notification> notification);
}
