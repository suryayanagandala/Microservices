using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Features.UpdateCart
{
    public record UpdateCartResponse
    {
        public bool IsSucess { get; init; }

        public string Message { get; init; } = null!;

        public DateTimeOffset UpdatedAt { get;init; }
    }
}
