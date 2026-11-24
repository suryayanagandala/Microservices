using Notifications.ApplicationLayer.Interfaces;
using Notifications.CoreModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Notifications.DbAccessLayer.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public INotificationRepository NotificationRepository { get; }

        public UnitOfWork(ApplicationDbContext db,INotificationRepository notificationRepository)
        {
            NotificationRepository = notificationRepository;
            _db = db;
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }

        public Task<int> SaveChanges(CancellationToken cancellationToken = default)
        {
            return _db.SaveChangesAsync(cancellationToken);
        }
    }
}
