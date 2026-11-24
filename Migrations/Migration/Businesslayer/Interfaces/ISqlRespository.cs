using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Application.Interfaces
{
    public interface ISqlRepository<TEntity>
    where TEntity : class
    {
        Task<List<TEntity>> GetBatchAsync(
            int batchSize,
            int skip,
            CancellationToken cancellationToken = default);

        //Task<List<TEntity>> GetBatchAsync(object lastkey, int batchSize, CancellationToken cancellationToken = default);
    }
}
