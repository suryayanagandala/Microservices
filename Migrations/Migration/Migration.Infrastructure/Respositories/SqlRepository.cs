using Microsoft.EntityFrameworkCore;
using Migration.Application.Interfaces;
using Migration.Infrastructure.DbSetUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Infrastructure.Respositories
{
    public class SqlRepository<TEntity> : ISqlRepository<TEntity> where TEntity : class
    {
        private readonly SqlServerDbContext _sql;

        public SqlRepository(SqlServerDbContext sql)
        {
            _sql = sql;
        }

        //public async Task<List<TEntity>> GetBatchAsync(object lastkey, int batchSize, CancellationToken cancellationToken = default)
        //{
        //    return await _sql.
        //}
        public async Task<List<TEntity>> GetBatchAsync(int batchSize, int skip, CancellationToken cancellationToken = default)
        {
            return await _sql.Set<TEntity>().AsNoTracking().Skip(skip).
                Take(batchSize).ToListAsync(cancellationToken);
        }
    }
}
