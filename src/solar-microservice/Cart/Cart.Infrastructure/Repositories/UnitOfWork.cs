using Cart.ApplicationLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Infrastructure.Respositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CartDbContext _db;
        public ICartRepository ICartRepository { get; }

        public UnitOfWork(CartDbContext db,ICartRepository cartRepository)
        {
            _db = db;
            ICartRepository = cartRepository;
        }

       

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveChanges(CancellationToken cancellationToken)
        {
            return await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
