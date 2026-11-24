using Cart.ApplicationLayer.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace CART.API.Hubs
{
    public class CartNotificationService(IHubContext<CartHub> hubContext) : ICartNotificationService
    {

        public async Task SendCartCount(string userId, int count)
        {
            await hubContext.Clients.User(userId).SendAsync("CartCountUpdated", count);
        }
    }
}
