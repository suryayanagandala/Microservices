using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.BusinessLayer.Features.RestoreProduct
{
    public record RestoreProductResponse
    {
        public bool IsSucess {  get; init; }
    }
}
