using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.ApplicationLayer.Interfaces
{
    public interface ICartNotificationService
    {
        Task SendCartCount(string userId, int count);
    }
}
