using Microsoft.EntityFrameworkCore;
using Notifications.CoreModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Notifications.CoreModels.Configurations
{
    public class NotificationsConfiguration : IEntityTypeConfiguration<Notification>
    {
        void IEntityTypeConfiguration<Notification>.Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");
            builder.HasKey(x => x.NotificationId);

            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.Message).IsRequired();

            builder.Property(x => x.CreatedAt).IsRequired();
            

        }
    }
}
