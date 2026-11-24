using BuildingBlocks.Authentication_.Abstractions;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using Orders.ApplicationLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.BusinessLayer.Features.GetInternalOrderResponse
{
    internal class GetInternalOrderHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetInternalOrderCommand, GetInternalOrderResult>
    {
        public async Task<GetInternalOrderResult> Handle(GetInternalOrderCommand request, CancellationToken cancellationToken)
        {


            var order = await unitOfWork.Orders.GetByIdAsync(request.orderId,cancellationToken);

            if (order is null)
                throw new NotFoundException($"Order {request.orderId} not found.");

            return new GetInternalOrderResult(
                order.OrderId,
                order.UserId,
                order.TotalAmount,
                order.Status);
        }
    }
}
