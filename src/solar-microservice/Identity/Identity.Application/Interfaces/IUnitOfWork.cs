using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IRoleInterface roleInterface { get; }

        IUserInterface userInterface { get; }

        IRefreshTokenInterface refreshTokenInterface { get; }

        Task<int> SaveChanges(CancellationToken cancellationToken);
    }
}
