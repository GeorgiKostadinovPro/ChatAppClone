namespace ChatAppClone.Middlewares
{
    using System;
    using System.Net;
    using System.Text.Json;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class GeneralExceptionHandlerMiddleware : AbstractExceptionHandlerMiddleware
    {
        public GeneralExceptionHandlerMiddleware(RequestDelegate _next, ILogger<AbstractExceptionHandlerMiddleware> _logger) 
            : base(_next, _logger) {}

        public override (HttpStatusCode code, string payload) GetResponse(Exception exception)
        {
            HttpStatusCode statusCode;

            switch (exception)
            {
                case InvalidOperationException _:
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            var payload = JsonSerializer.Serialize(new
            {
                success = false,
                error = exception.Message
            });

            return (statusCode, payload);
        }
    }
}
