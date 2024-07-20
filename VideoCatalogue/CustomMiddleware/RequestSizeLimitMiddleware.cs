using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace VideoCatalogue.CustomMiddleware
{

    public class RequestSizeLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RequestSizeLimitOptions _options;

        public RequestSizeLimitMiddleware(RequestDelegate next,IOptions<RequestSizeLimitOptions> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments(_options.PathToLimit) && context.Request.ContentLength > _options.MaxRequestBodySize)
            {
                context.Response.StatusCode = StatusCodes.Status413PayloadTooLarge;
                await context.Response.WriteAsync(context.Response.StatusCode + $" The request body is too large. The maximum allowed size is {_options.MaxRequestBodySize / (1024 * 1024)} MB.");
                return;
            }

            await _next(context);
        }


    }
    public class RequestSizeLimitOptions
    {
        public long MaxRequestBodySize { get; set; }
        public string PathToLimit { get; set; }
    }

}
