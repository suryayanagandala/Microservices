using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Features.ClearCart
{
    public record ClearCartCommand(): ICommand<ClearCartResult>;

    public record ClearCartResult(bool IsSuccess,string Message);
}
