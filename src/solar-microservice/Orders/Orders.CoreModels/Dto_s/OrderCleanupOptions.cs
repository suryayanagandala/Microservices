using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.CoreModels.Dto_s
{
    public sealed class OrderCleanupOptions
    {
        public const string SectionName = "OrderCleanup";

        public int ExpiryMinutes { get; set; }

        public int IntervalMinutes { get; set; }
    }
}
