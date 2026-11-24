using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.CoreModels.Entites
{
    public sealed class Categories
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string Slug { get; set; }= string.Empty;

        public int? CategoryParentId { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public string? DeletedReason { get; set; }

        public DateTimeOffset? DeletedAt { get; set; }

        public string? ImageUrl { get; set; }

        public Categories? ParentCategory { get; set; }

        public ICollection<Categories> ChildCategories {  get; set; }=new List<Categories>();

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
