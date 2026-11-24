using Migration.Application.Interfaces;
using Migration.Infrastructure.Respositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Busniesslayer.Service
{
    public abstract class MigrationBase<TEntity> : IDataMigrationService where TEntity : class
    {
        private readonly IPostgresRepository<TEntity> postgresRepository;
        private readonly ISqlRepository<TEntity> sqlRepository;

        protected MigrationBase(IPostgresRepository<TEntity> postgresRepository,ISqlRepository<TEntity> sqlRepository)
        {
            this.postgresRepository = postgresRepository;
            this.sqlRepository = sqlRepository;
        }

        protected abstract int BatchSize { get; }
        public virtual async Task MigrateAsync(CancellationToken cancellationToken=default)
        {
            var skip = 0;
            while (true)
            {
                var records = await this.sqlRepository.GetBatchAsync(BatchSize, skip, cancellationToken);

                if (records.Count == 0)
                    break;

                await TransformAsync(records);
                await this.postgresRepository.InsertBatchAsync(records,cancellationToken);

                skip += BatchSize;

                Console.WriteLine($"{typeof(TEntity).Name} : {skip} rows migrated");
            }
            Console.WriteLine($"{typeof(TEntity).Name} migration completed.");
        }

        protected virtual Task TransformAsync(List<TEntity> records)
        {
            return Task.CompletedTask;
        }
    }
}
