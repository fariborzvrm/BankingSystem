using BankingSystem.Application.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception occurred. TraceId: {TraceId}", httpContext.TraceIdentifier);

            var problemDetails = CreateProblemDetails(httpContext, exception);

            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        private static ProblemDetails CreateProblemDetails(HttpContext httpContext, Exception exception)
        {
            var problemDetails = exception switch
            {
                NotFoundAppException ex => new ProblemDetails
                {
                    Title = "Resource Not Found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound,
                    Type = "https://httpstatuses.com/404",
                    Instance = httpContext.Request.Path
                },

                ValidationException ex => new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://httpstatuses.com/400",
                    Instance = httpContext.Request.Path
                },

                UnauthorizedAccessException ex => new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = ex.Message,
                    Status = StatusCodes.Status401Unauthorized,
                    Type = "https://httpstatuses.com/401",
                    Instance = httpContext.Request.Path
                },

                _ => new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred.",
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "https://httpstatuses.com/500",
                    Instance = httpContext.Request.Path
                }
            };

            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

            return problemDetails;
        }
    }
}
