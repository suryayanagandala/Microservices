using BuildingBlocks.CQRS;
using Payments.CoreModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.BusinessLayer.Features.CreatePayment
{
    public sealed record CreatePaymentCommand(int OrderId,PaymentMethod PaymentMethod)
    : ICommand<int>;


}
