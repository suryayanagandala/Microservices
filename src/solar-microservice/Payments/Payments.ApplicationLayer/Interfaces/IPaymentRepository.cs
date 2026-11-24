using Payments.CoreModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.ApplicationLayer.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddPayment(Payment payment);

        Task<Payment?> GetPaymentByOrderId(int orderId,CancellationToken cancellationToken);

        Task<bool> ExistsPayment(int orderId,CancellationToken cancellationToken);
    }
}
