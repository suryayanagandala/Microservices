using BuildingBlocks.Exceptions.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Minio.Exceptions;

namespace BuildingBlocks.Utility
{
    public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception exception,
            CancellationToken cancellationToken)
        {
            logger.LogError(exception, exception.Message);

            (string Detail, string Title, int StatusCode) details = exception switch
            {
                InternalServerException => (
                    exception.InnerException?.Message??exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError
                ),

                DbUpdateException=>(
                 exception.InnerException?.Message??exception.Message,
                 exception.GetType().Name,
                 context.Response.StatusCode=StatusCodes.Status409Conflict
                ),

                FluentValidation.ValidationException => (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status400BadRequest
                ),

                BadRequestException => (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status400BadRequest
                ),

                AlreadyExists => (
                   exception.Message,
                   exception.GetType().Name,
                   context.Response.StatusCode = StatusCodes.Status409Conflict
               ),

                NotFoundException => (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status404NotFound
                ),
                UnauthorizedException => (
                    exception.Message,
                    exception.GetType().Name,
                    StatusCodes.Status401Unauthorized
                ),

                _ => (
                    exception.Message,
                    exception.GetType().Name,
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError
                )
            };


            var problemDetails = new ProblemDetails
            {
                Title = details.Title,
                Detail = details.Detail,
                Status = details.StatusCode,
                Instance = context.Request.Path
            };


            problemDetails.Extensions.Add(
                "traceId",
                context.TraceIdentifier
            );


            // FluentValidation errors
            if (exception is FluentValidation.ValidationException validationException)
            {
                problemDetails.Extensions.Add(
                    "ValidationErrors",
                    validationException.Errors
                        .Select(x => x.ErrorMessage)
                        .ToList()
                );
            }

            var errors = new List<string>();

            if (exception is ValidationException validationEx)
            {
                errors = validationEx.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList();
            }
            else
            {
                errors.Add(details.Detail);
            }

            var response =
                ApiResponse<object>.Failure(errors, details.Title);

            await context.Response.WriteAsJsonAsync(
                response,
                cancellationToken);


            return true;
        }
    }
}