using BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Features.CheckOut
{
    public record CheckOutCommand
    (
        string FullName,
        string PhoneNumber,
        string DeliveryAddress
        //decimal TotalAmount
        ):ICommand<CheckOutResult>;
}
