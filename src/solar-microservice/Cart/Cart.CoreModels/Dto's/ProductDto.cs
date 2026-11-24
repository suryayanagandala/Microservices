using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.CoreModels.Dto_s
{
    public record ProductDto
    {
        public int ProductId { get; init; }

        public string ProductName { get; init; } = default!;

        public decimal Price { get; init; }

        public bool IsActive { get; init; }

        public string? ImageUrl { get; init; }
    }
}
