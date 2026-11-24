using Orders.CoreModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.BusinessLayer.Features.GetInternalOrderResponse
{
    public sealed record GetInternalOrderResult(
    int OrderId,
    string UserId,
    decimal TotalAmount,
    OrderStatus Status);
}
