using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.CoreModels.Entites
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public int? ParentCategoryId { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Description { get; set; } = null!;
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? DeletedReason { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
        public string? ImageUrl { get; set; }
    }
}
