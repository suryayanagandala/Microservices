using BuildingBlocks.Utility;
using Carter;
using Identity.BusinessLayer.Features.AddRole;
using Identity.BusinessLayer.Features.CreateUser;
using Identity.BusinessLayer.Features.ForGotPassword;
using Identity.BusinessLayer.Features.LoginUser;
using Identity.BusinessLayer.Features.RefreshToken;
using Identity.BusinessLayer.Features.ResetPassword;
using Identity.BusinessLayer.Features.UpdateProfile;
using Identity.BusinessLayer.Features.VerifyOtp;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Minio.Helper;

namespace Identity.Api.IndentityApiEndPoints
{
    public class AuthenticationEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/auth/register", async (RegisterCommand request, ISender sender) =>
            {
                var resul = await sender.Send(request);
                return Results.Ok(ApiResponse<CreateUserResponse>.Success(resul)
                    );

            }).WithName("CreateProduct").Produces<CreateUserResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest).WithSummary("Create User");


            app.MapPost("/role/AddUser", async (RoleHandlerRequest request, ISender sender) =>
            {
                var response=await sender.Send(request);
                return Results.Ok(ApiResponse<RoleResponse>.Success(response,"succesfully added one role"));
            });

            app.MapPut("/api/auth/update/{userId}", async (string userId,UpdateProfileRequest request, ISender sender) =>
            {
                //Because userId comes from the route, you don't need to send it in the JSON body.
                var requestId = request with
                {
                    userId=userId
                };
                var response = await sender.Send(requestId);
                return Results.Ok(ApiResponse<UpdateProfileResponse>.Success(response, "updated sucesfully"));
            });

            app.MapPost("/users/forgotpassword", async (ForgotPasswordReq request, ISender sender) =>
            {
                var response = await sender.Send(request);
                return Results.Ok(ApiResponse<string>.Success(response.otptext, "forgotpassword")); 
            });

            app.MapPost("/users/verifyOtp", async (VerifyOtpRequest request, ISender sender) =>
            {
                var response = await sender.Send(request);
                return Results.Ok(ApiResponse<VerifyOtpResponse>.Success(response, "verify otp"));
            });

            app.MapPost("/users/Resetpassword", async (ResetPasswordReq request, ISender sender) =>
            {
                var response = await sender.Send(request);

                return Results.Ok(ApiResponse<ResetPasswordResponse>.Success(response, "reset password created successfully"));
            });

            app.MapPost("/users/updatePassword", async (UpdateProfileRequest request, ISender sender) =>
            {
                var response=await sender.Send(request);

                return Results.Ok(ApiResponse<UpdateProfileResponse>.Success(response,"Password updated succesfully"));
            });

            app.MapPost("/users/login", async (LoginUserRequest request, ISender sender) =>
            {
                var response = await sender.Send(request);
                return Results.Ok(ApiResponse<LoginUserResponse>.Success(response, "login successfully created"));
            });

            app.MapPost("/users/refreshtoken", async (RefreshTokenRequest request, ISender sender) =>
            {
                var response = await sender.Send(request);
                return Results.Ok(ApiResponse<LoginUserResponse>.Success(response, "refresh token  successfully created"));
            });
        }
    }
}
