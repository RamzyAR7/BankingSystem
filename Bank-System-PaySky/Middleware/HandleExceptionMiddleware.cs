using System.Net;
using System.Text.Json;
using System.Transactions;
using Bank_System_PaySky.Exceptions;
using Bank_System_PaySky.Exeptions;
using Microsoft.EntityFrameworkCore;

namespace Bank_System_PaySky.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var requestId = Guid.NewGuid().ToString();
            context.Items["RequestId"] = requestId;
            context.Response.Headers["X-Request-ID"] = requestId;

            try
            {
                _logger.LogInformation("Request {RequestId} Received: {Method} {Path}",
                                       requestId,
                                       context.Request.Method,
                                       context.Request.Path);

                await next(context);

                _logger.LogInformation("Response {RequestId} Sent: {StatusCode}",
                                       requestId,
                                       context.Response.StatusCode);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, requestId);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, string requestId)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                AccountNotFoundException => (int)HttpStatusCode.NotFound,
                InvalidAccountOperationException => (int)HttpStatusCode.BadRequest,
                InvalidAccountNumberException => (int)HttpStatusCode.BadRequest,
                TransactionException => (int)HttpStatusCode.BadRequest,
                DbUpdateExceptionHandle => (int)HttpStatusCode.BadRequest,
                Exception => (int)HttpStatusCode.InternalServerError,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var response = new ErrorDetails
            {
                Code = context.Response.StatusCode.ToString(),
                Message = exception.Message,
                RequestId = requestId
            };

            _logger.LogError(exception, "Error in Request {RequestId}: {Message}", requestId, exception.Message);

            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }
    }

    public class ErrorDetails
    {
        public string Code { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string RequestId { get; set; } = null!;
    }
}
