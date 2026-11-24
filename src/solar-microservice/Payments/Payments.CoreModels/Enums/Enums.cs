using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.CoreModels.Enums
{
    public enum PaymentStatus
    {
        Pending ,
        Success,
        Failed,
        Refunded,
    }

    public enum PaymentMethod
    {
        Upi,
        Card,
        NetBanking,
        CashOnDelivery,
    }
}
