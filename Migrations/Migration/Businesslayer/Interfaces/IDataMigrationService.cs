using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Application.Interfaces
{
    public interface IDataMigrationService
    {
        Task MigrateAsync(CancellationToken cancellationToken=default);
    }
}
