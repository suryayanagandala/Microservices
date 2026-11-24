using Notifications.CoreModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.CoreModels.Entities
{
    public class Notification
    {
        public int NotificationId {  get; set; }

        public string UserId { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string Title {  get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty; 

        public DateTimeOffset CreatedAt {  get; set; }

        public bool IsRead { get; set; } = false;

        public bool IsActive { get; set; } = true;


    }
}
