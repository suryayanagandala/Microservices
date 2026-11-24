using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.ApplicationLayer.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        ICategoryInterface CategoryInterface { get;}

        IProductInterface ProductInterface { get;}

        Task<int> SaveChanges(CancellationToken cancellationToken = default);
    }
}
