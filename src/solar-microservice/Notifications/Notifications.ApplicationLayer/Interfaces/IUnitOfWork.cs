using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.ApplicationLayer.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
         INotificationRepository NotificationRepository { get; }

        Task<int> SaveChanges(CancellationToken cancellationToken=default);
    }
}
