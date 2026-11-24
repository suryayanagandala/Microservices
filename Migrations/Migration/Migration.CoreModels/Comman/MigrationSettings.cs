using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.CoreModels.Comman
{
    public class MigrationSettings
    {
        public int BatchSize { get; set; } = 5000;
    }
}
