using Identity.Application.Interfaces;
using Identity.CoreModels.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IRoleInterface roleInterface { get; }

        public IUserInterface userInterface { get; }

        public IRefreshTokenInterface refreshTokenInterface { get; }

        public UnitOfWork(ApplicationDbContext db,IRoleInterface roleInterface,IUserInterface userInterface,
            IRefreshTokenInterface refreshTokenInterface)
        {
            _db=db;
            this.roleInterface = roleInterface;
            this.userInterface = userInterface;
            this.refreshTokenInterface = refreshTokenInterface;
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveChanges(CancellationToken cancellationToken)
        {
           return  await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
