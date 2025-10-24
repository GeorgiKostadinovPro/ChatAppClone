namespace ChatAppClone.Middlewares
{
    using System.Net;

    using ChatAppClone.Common.Constants;

    public abstract class AbstractExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<AbstractExceptionHandlerMiddleware> logger;

        public abstract (HttpStatusCode code, string payload) GetResponse(Exception exception);

        protected AbstractExceptionHandlerMiddleware(RequestDelegate _next, ILogger<AbstractExceptionHandlerMiddleware> _logger)
        {
            this.next = _next;
            this.logger = _logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, MiddlewareConstants.ErrorDuringExecutingPath, context.Request.Path.Value);

                var (status, payload) = this.GetResponse(ex);

                await this.HandleResponseAsync(context, status, payload);
            }
        }

        private async Task HandleResponseAsync(HttpContext context, HttpStatusCode status, string payload)
        {
            bool isUserApi = context.Request.Path.Value?.StartsWith(MiddlewareConstants.UserApiPath, StringComparison.OrdinalIgnoreCase) == true;

            context.Response.Clear();

            if (isUserApi)
            {
                context.Response.StatusCode = (int)status;
                context.Response.ContentType = MiddlewareConstants.ApplicationJson;
                await context.Response.WriteAsync(payload);
            }
            else
            {
                int code = (int)status;

                string redirectUrl = status switch
                {
                    HttpStatusCode.BadRequest => MiddlewareConstants.RedirectURLs[code],
                    HttpStatusCode.Unauthorized => MiddlewareConstants.RedirectURLs[code],
                    HttpStatusCode.Forbidden => MiddlewareConstants.RedirectURLs[code],
                    HttpStatusCode.NotFound => MiddlewareConstants.RedirectURLs[code],
                    _ => MiddlewareConstants.RedirectURLs[code]
                };

                context.Response.Redirect(redirectUrl);
            }
        }
    }
}
