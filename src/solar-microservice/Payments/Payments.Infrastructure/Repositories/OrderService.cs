using Payments.ApplicationLayer.Interfaces;
using Payments.CoreModels.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Infrastructure.Repositories
{
    public class OrderService(HttpClient httpClient) : IOrderService
    {
        public async Task<OrderResponse?> GetOrderDetails(int orderId, CancellationToken cancellationToken = default)
        {
            return await httpClient.GetFromJsonAsync<OrderResponse>($"orders/internal/{orderId}", cancellationToken);
        }


    }
}
