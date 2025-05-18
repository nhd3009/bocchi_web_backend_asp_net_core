using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bocchiwebbackend.Models.Common;

namespace bocchiwebbackend.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiResponse<object>();

            switch (exception)
            {
                case KeyNotFoundException:
                    response = ApiResponse<object>.NotFoundResponse("Resource not found");
                    break;
                case UnauthorizedAccessException:
                    response = ApiResponse<object>.UnauthorizedResponse();
                    break;
                default:
                    response = ApiResponse<object>.ErrorResponse("An unexpected error occurred", "SERVER_ERROR", 500);
                    break;
            }

            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}