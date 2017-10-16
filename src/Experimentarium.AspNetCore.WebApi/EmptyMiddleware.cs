using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class EmptyMiddleware
{
    private readonly RequestDelegate _next;

    // Only one instance is created per application => don't inject per-request dependencies here
    public EmptyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // Add any needed per-request dependencies here as arguments and they will be resolved using DI
    public Task Invoke(HttpContext context, ILogger<EmptyMiddleware> logger) 
    {
        // do something
        logger.LogInformation("EmptyMiddleware invoked");

        // request/response info: context.Request / context.Response

        // Call the next delegate/middleware in the pipeline
        return this._next(context);
    }
}

    public static class EmptyMiddlewareEstensions
    {
        public static IApplicationBuilder UseEmptyMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EmptyMiddleware>();
        }
    }