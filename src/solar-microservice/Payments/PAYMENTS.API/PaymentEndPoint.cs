using BuildingBlocks.Utility;
using Carter;
using MediatR;
using Payments.BusinessLayer.Features.CreatePayment;
using Payments.BusinessLayer.Features.GenerateQr;

namespace PAYMENTS.API
{
    public class PaymentEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var payments = app.MapGroup("/payments").WithTags("Payments");

            payments.MapPost("/payments",async (CreatePaymentCommand command,ISender sender) =>
            {
                    var paymentId = await sender.Send(command);

                    return ApiResponse<object>.Success(new
                    {
                        PaymentId = paymentId,
                        Message = "Payment completed successfully."
                    });
            }).RequireAuthorization()
            .WithSummary("After creating of orders then only payment happened");

            app.MapPost(
            "/payments/generate-qr",
            async (
                GenerateQrCommand command,
                ISender sender) =>
            {
                var result = await sender.Send(command);

                return ApiResponse<GenerateQrResponse>.Success(result);
            })
        .RequireAuthorization()
        .WithTags("Payments");

        }
    }
}
