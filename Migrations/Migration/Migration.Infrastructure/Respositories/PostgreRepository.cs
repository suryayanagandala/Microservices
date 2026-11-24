using Migration.Application.Interfaces;
using Migration.Infrastructure.DbSetUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Infrastructure.Respositories
{
    public class PostgreRepository<TEntity> : IPostgresRepository<TEntity> where TEntity : class
    {
        private readonly PostgresDbContext _pg;

        public PostgreRepository(PostgresDbContext pg)
        {
            _pg = pg;
        }
        public async Task InsertBatchAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await _pg.Set<TEntity>().AddRangeAsync(entities,cancellationToken);
            await _pg.SaveChangesAsync(cancellationToken);
        }
    }
}
