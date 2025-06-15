using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;

namespace ApiRefactor.Extensions
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // proceed to the controller
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Not Found");
                await WriteProblemDetailsAsync(context, 404, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");
                await WriteProblemDetailsAsync(context, 500, "An unexpected error occurred.");
            }
        }

        private static Task WriteProblemDetailsAsync(HttpContext context, int statusCode, string message)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = ReasonPhrases.GetReasonPhrase(statusCode),
                Detail = message
            };

            return context.Response.WriteAsJsonAsync(problem);
        }
    }
}
