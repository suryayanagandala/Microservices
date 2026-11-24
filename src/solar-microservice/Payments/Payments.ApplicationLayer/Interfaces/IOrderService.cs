using Payments.CoreModels.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.ApplicationLayer.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponse?> GetOrderDetails(int orderId,CancellationToken cancellationToken = default);
    }
}
