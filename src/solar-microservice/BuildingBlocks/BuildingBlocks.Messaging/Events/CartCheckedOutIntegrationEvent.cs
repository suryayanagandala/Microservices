using BuildingBlocks.Messaging.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Events
{
    public sealed record CartCheckedOutIntegrationEvent(
        string UserId,string FullName,string PhoneNumber,
        string DeliveryAddress,
        decimal TotalAmount,List<CartItemEventDto> CartItemEventDtos

        ):IntegrationEvent;


    public record CartItemEventDto
    (
         int ProductId ,

         int Quantity ,
         decimal UnitPrice );
}   
