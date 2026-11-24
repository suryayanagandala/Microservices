using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Features.AddToCart
{
    public record  AddToCartResponse(bool IsSuccess,
    string Message);
}
