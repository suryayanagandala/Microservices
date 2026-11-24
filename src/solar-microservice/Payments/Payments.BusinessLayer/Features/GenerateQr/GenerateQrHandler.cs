using BuildingBlocks.CQRS;
using Payments.ApplicationLayer.Interfaces;
using Payments.BusinessLayer.Helpers;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.BusinessLayer.Features.GenerateQr
{
    internal sealed class GenerateQrHandler(IOrderService orderService)
    : ICommandHandler<GenerateQrCommand, GenerateQrResponse>
    {
        public async Task<GenerateQrResponse> Handle(
            GenerateQrCommand request,
            CancellationToken cancellationToken)
        {
            var order = await orderService.GetOrderDetails(
                request.OrderId,
                cancellationToken);

            if (order is null)
                throw new Exception("Order not found.");

            var transactionId = TransactionHelper.Generate();

            var qrText =
                $"SOLARX|ORDER:{order.OrderId}|USER:{order.UserId}|AMOUNT:{order.TotalAmount}|TXN:{transactionId}";

            using var generator = new QRCodeGenerator();

            using var data = generator.CreateQrCode(
                qrText,
                QRCodeGenerator.ECCLevel.Q);

            var qrCode = new PngByteQRCode(data);

            byte[] bytes = qrCode.GetGraphic(20);

            return new GenerateQrResponse(
                order.OrderId,
                order.TotalAmount,
                transactionId,
                300,
                $"data:image/png;base64,{Convert.ToBase64String(bytes)}");
        }
    }
}
