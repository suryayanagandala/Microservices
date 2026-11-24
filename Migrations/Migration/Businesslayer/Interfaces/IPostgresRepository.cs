using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Application.Interfaces
{
    public interface IPostgresRepository<TEntity>
    where TEntity : class
    {
        Task InsertBatchAsync(
            List<TEntity> entities,
            CancellationToken cancellationToken = default);
    }
}
