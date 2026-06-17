using System.Text.Json;

namespace Amazon.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        private readonly ILogger<ExceptionMiddleware> logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            this.next = next;

            this.logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An unhandled exception occurred.");

                context.Response.StatusCode = 500;

                context.Response.ContentType =
                    "application/json";

                var response =
                    new
                    {
                        Message =
                            "An unexpected error occurred.",

                        Error =
                            ex.Message
                    };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }
        }
    }
}