using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Features.CheckOut
{
    public record CheckOutResult
    {
        public bool IsSucess { get; init; }

        public string Message { get; init; }=string.Empty;

        public Guid EventId {  get; init; }=Guid.Empty;
    }
}
