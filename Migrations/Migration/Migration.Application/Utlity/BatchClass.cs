using Microsoft.EntityFrameworkCore;
using Migration.Infrastructure.DbSetUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Application.Utlity
{
    public   class BatchClass(PostgresDbContext _pg)
    {
        public async Task BatchInsert<T>(DbSet<T> table,List<T> data) where T:class
        {
            const int batchSize = 500;

            for(int i = 0;i<data.Count; i += 500)
            {
                var batch = data.Skip(i).Take(batchSize);
                await table.AddRangeAsync(batch);
                await _pg.SaveChangesAsync();
            }
        }
    }
}
