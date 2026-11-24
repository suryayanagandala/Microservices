using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using BuildingBlocks.Messaging.Contracts;
using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Messaging.Topology;
using Microsoft.Extensions.Logging;
using Payments.ApplicationLayer.Interfaces;
using Payments.BusinessLayer.Helpers;
using Payments.CoreModels.Entities;
using Payments.CoreModels.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.BusinessLayer.Features.CreatePayment
{
    public class CreatePaymentHandler(IUnitOfWork unitOfWork,IEventBus eventBus,IOrderService orderService,
        ILogger<CreatePaymentHandler> logger) : ICommandHandler<CreatePaymentCommand, int>
    {
        public async Task<int> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var order=await orderService.GetOrderDetails(request.OrderId);

            if (order is null) throw new NotFoundException("Order not found");
            
            if(order.Status!="PAYMENT_PENDING") throw new BadRequestException(
                $"Payment cannot be made because order status is {order.Status}");

            //DUPLICATE PAYMENT

            var alreadyPaid = await unitOfWork.paymentRepository.ExistsPayment(request.OrderId, cancellationToken);

            if (alreadyPaid)
                throw new BadRequestException(
                    "Payment already exists for this order.");

            var payment = new Payment
            {
                OrderId = order.OrderId,
                Amount = order.TotalAmount,
                PaymentMethod = request.PaymentMethod.ToString(),
                PaymentStatus = PaymentStatus.Success.ToString(),
                TransactionId = TransactionHelper.Generate(),
                PaymentDate = DateTimeOffset.UtcNow,
                IsActive = true
            };

            await unitOfWork.paymentRepository.AddPayment(payment);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var integrationEvent = new PaymentCompletedIntegrationEvent(
                payment.PaymentId,
                payment.OrderId,
                order.UserId,
                payment.Amount,
                payment.TransactionId,
                payment.PaymentMethod.ToString());

            await eventBus.PublishAsync(integrationEvent, RoutingKeys.Payment.PaymentCreated, cancellationToken);

            logger.LogInformation($"");

            return payment.PaymentId;
        }
    }
}
