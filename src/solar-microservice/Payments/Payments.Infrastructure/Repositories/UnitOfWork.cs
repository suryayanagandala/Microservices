using Payments.ApplicationLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Infrastructure.Repositories
{
    public class UnitOfWork(PaymentsDbContext dbContext,IPaymentRepository paymentRepository) : IUnitOfWork
    {

        public IPaymentRepository paymentRepository { get; } = paymentRepository;

        

        public void Dispose()
        {
            dbContext.Dispose();

            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
            =>await dbContext.SaveChangesAsync(cancellationToken);
        
    }
}
