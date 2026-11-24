using Microsoft.EntityFrameworkCore;
using Payments.ApplicationLayer.Interfaces;
using Payments.CoreModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Infrastructure.Repositories
{
    public class PaymentRespository(PaymentsDbContext dbContext) : IPaymentRepository
    {
        public Task AddPayment(Payment payment)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsPayment(int orderId, CancellationToken cancellationToken)
        {
            return await dbContext.Payments.AnyAsync(x=>x.OrderId == orderId&&x.IsActive, cancellationToken);
        }

        public Task<Payment?> GetPaymentByOrderId(int orderId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
