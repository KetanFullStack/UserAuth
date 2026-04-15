using System.Net;
using System.Text.Json;
using UserAuth.Application.Common;

namespace UserAuth.API
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled Exception Occurred");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            var message = ex.Message;
            var errors = new List<string>();
            context.Response.StatusCode = (int)statusCode;

            var apiResponse = new ApiResponse<string>
            {
                Success = false,
                Message = message,
                Errors = errors
            };

            return context.Response.WriteAsJsonAsync(apiResponse);
        }
    }
}
