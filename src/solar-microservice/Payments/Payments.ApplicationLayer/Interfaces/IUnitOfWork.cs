using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.ApplicationLayer.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IPaymentRepository paymentRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
