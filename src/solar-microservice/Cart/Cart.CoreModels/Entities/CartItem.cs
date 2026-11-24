using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.CoreModels.Entities
{
    public sealed class CartItem
    {
        public int CartItemId { get; set; }

        public int CartId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTimeOffset? UpdatedAt { get; set; }

        public Carts Cart { get; set; } = default!;
    }
}
