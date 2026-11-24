using BuildingBlocks.Utility;
using Carter;
using MediatR;
using Notifications.BusinessLayer.Features.GetNotifications;
using Notifications.BusinessLayer.Features.MarkAsAllRead;

namespace Notifications.API.EndPoints
{
    public class NotificationsEndPoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/notifications/getAll/{userId?}",
            async (string? userId, ISender sender) =>
            {
                var request = new GetNotificationsRequest(userId);

                var response = await sender.Send(request);

                return Results.Ok(ApiResponse<GetNotificationsResponse>.Success(response));
            });

            app.MapGet("/notification/markallasread", async (ISender sender) =>
            {
                var response = await sender.Send(new MarkAsAllReadRequest());
                return Results.Ok(ApiResponse<MarkAsAllReadResponse>.Success(response));
            }).RequireAuthorization();

            app.MapGet("/claims", (HttpContext context) =>
            {
                return Results.Ok(context.User.Claims.Select(c => new
                {
                    c.Type,
                    c.Value
                }));
            }).RequireAuthorization();
        }
    }
}
