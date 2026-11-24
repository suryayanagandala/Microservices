using Products.ApplicationLayer.Interfaces;
using Products.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.DbAccessLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductDbContext _db;
        public ICategoryInterface CategoryInterface { get; }

        public IProductInterface ProductInterface { get; }


        public UnitOfWork(ProductDbContext db,ICategoryInterface categoryInterface
            ,IProductInterface productInterface)
        {
            _db = db;
            CategoryInterface = categoryInterface;
            ProductInterface = productInterface;
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveChanges(CancellationToken cancellationToken = default)
        {
            return await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
