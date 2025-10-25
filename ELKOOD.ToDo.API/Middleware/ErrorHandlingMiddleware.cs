using System.Net;
using System.Text.Json;

namespace ELKOOD.ToDo.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
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

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch (exception)
            {
                case UnauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    result = "Unauthorized access";
                    break;
                case ArgumentException:
                    code = HttpStatusCode.BadRequest;
                    result = exception.Message;
                    break;
                case InvalidOperationException:
                    code = HttpStatusCode.BadRequest;
                    result = exception.Message;
                    break;
                default:
                    code = HttpStatusCode.InternalServerError;
                    result = "An internal server error occurred";
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if (string.IsNullOrEmpty(result))
            {
                result = JsonSerializer.Serialize(new { error = exception.Message });
            }
            else
            {
                result = JsonSerializer.Serialize(new { error = result });
            }

            return context.Response.WriteAsync(result);
        }
    }
}