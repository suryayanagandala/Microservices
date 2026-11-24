using BuildingBlocks.Authentication_.Abstractions;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions.Exceptions;
using BuildingBlocks.Messaging.Contracts;
using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Messaging.Topology;
using Cart.ApplicationLayer.Interfaces;
using Microsoft.Identity.Client;
using Minio.DataModel;
using Minio.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.BusinessLayer.Features.CheckOut
{
    internal class CheckOutHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser, IEventBus eventBus,IProductService productService)
        : ICommandHandler<CheckOutCommand, CheckOutResult>
    {
        public async Task<CheckOutResult> Handle(CheckOutCommand request, CancellationToken cancellationToken)
        {
            //Get Active Cart

            var userId = currentUser.UserId;

            if (userId is null) throw new UnauthorizedAccessException();

            var cart = await unitOfWork.ICartRepository.GetCartWithItems(userId);

            if (cart is null) throw new NotFoundException("Cart not found");

            if(cart.UserId!=userId) throw new UnauthorizedAccessException();

            if (!cart.CartItems.Any()) throw new BadRequestException("Cart is empty");

            //get productId for productdetails

            var productIds = cart.CartItems.Select(x => x.ProductId).ToList();

            //get priduct details

            var products = await productService.GetProductsAsync(productIds,cancellationToken);


            //for time complexity
            var lookup = products.ToDictionary(x => x.ProductId);

            var items = new List<CartItemEventDto>();

            //calcualte total

            decimal total=0;

            foreach (var item in cart.CartItems)
            {
                if (!lookup.TryGetValue(item.ProductId, out var product))
                    throw new NotFoundException($"Product {item.ProductId} not found");

                total += item.Quantity * item.UnitPrice;

                items.Add(new CartItemEventDto(item.ProductId, item.Quantity, product.Price));



            }

            var integrationEvent = new CartCheckedOutIntegrationEvent(
                userId,request.FullName,request.PhoneNumber,
                request.DeliveryAddress,total,
                items
                );
            //publish event

            await eventBus.PublishAsync(integrationEvent, RoutingKeys.Carts.CheckOut, cancellationToken);

            return new CheckOutResult()
            {
                IsSucess = true,
                EventId=integrationEvent.EventId,
                Message= "Checkout request submitted successfully"
            };
        }
    }
}
