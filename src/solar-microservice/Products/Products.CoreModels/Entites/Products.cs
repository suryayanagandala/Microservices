using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.CoreModels.Entites
{
    public sealed class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public string Brand { get; set; } = null!;

        public string Type { get; set; } = null!;

        public int CapacityValue { get; set; }

        public string Capacity { get; set; } = null!;

        public decimal Price { get; set; }

        public decimal WarrantyYears { get; set; }

        public int Stock { get; set; }

        public string Description { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public int? CategoryId { get; set; }


        public string? DeletedReason { get; set; } 

        public DateTimeOffset? DeletedAt {  get; set; } 

        public Categories Category { get; set; }= null!;

        public string Unit {  get; set; } = null!;

        public ICollection<ProductImages> ProductImages { get; set; } = new List<ProductImages>();
    }
}
