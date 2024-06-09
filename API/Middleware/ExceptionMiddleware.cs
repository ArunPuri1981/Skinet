using System.Net;
using System.Text.Json;
using API.Error;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _hostEnvironment;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,

        IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _hostEnvironment.IsDevelopment()
                ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                : new ApiException((int)HttpStatusCode.InternalServerError);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };


                var json = JsonSerializer.Serialize(response, options);
                await httpContext.Response.WriteAsync(json);

            }
        }
    }
}