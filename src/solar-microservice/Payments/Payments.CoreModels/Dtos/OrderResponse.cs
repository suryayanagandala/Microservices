using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.CoreModels.Dtos
{
    public sealed record OrderResponse(
    int OrderId,
    string UserId,
    decimal TotalAmount,
    string Status);
}

