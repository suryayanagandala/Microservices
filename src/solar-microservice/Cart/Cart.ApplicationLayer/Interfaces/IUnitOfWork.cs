using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.ApplicationLayer.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        ICartRepository ICartRepository { get; }

       
        Task<int> SaveChanges(CancellationToken cancellationToken);
    }
}
