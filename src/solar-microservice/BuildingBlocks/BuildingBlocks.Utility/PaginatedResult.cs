using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Utility
{
    public class PaginatedResult
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }

    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
            = Enumerable.Empty<T>();

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }
    }

    public static class ApplicationConstants
    {
        public const int DefaultPageSize = 10;

        public const int MaxPageSize = 100;
    }
}
