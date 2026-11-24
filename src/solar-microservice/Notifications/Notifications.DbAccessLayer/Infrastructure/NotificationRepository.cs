using Microsoft.EntityFrameworkCore;
using Notifications.ApplicationLayer.Interfaces;
using Notifications.CoreModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.DbAccessLayer.Infrastructure
{
    public class NotificationRepository:INotificationRepository
    {
        private readonly ApplicationDbContext _db;

        public NotificationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddNotification(Notification notification)
        {
            await _db.Notifications.AddAsync(notification);
        }

        public async Task<bool> ExistUser(string userId)
        {
            return await _db.Notifications.AnyAsync(x=>x.UserId==userId);
        }

        public async Task<IReadOnlyList<Notification>> GetAllNotifications(string? userId)
        {
            IQueryable<Notification> query = _db.Notifications.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                query=query.Where(x=>x.UserId== userId);    
            }

            return await query.OrderByDescending(x=>x.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUserNotifications(string userId)
        {
            return await _db.Notifications.Where(x=>x.UserId== userId&&!x.IsRead).OrderByDescending(x=>x.CreatedAt).ToListAsync();
        }

    }
}
