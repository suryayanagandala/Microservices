using Notifications.CoreModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.ApplicationLayer.Interfaces
{
    public interface INotificationRepository
    {
        Task AddNotification(Notification notification);

        Task<IReadOnlyList<Notification>> GetAllNotifications(string? userId);

        Task<bool> ExistUser(string userId);

        Task<IEnumerable<Notification>> GetUserNotifications(string userId); 

        
    }
}
