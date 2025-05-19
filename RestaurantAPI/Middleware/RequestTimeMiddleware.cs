using System.Diagnostics;

namespace RestaurantAPI.Middleware;

public class RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger) : IMiddleware
{
    private Stopwatch stopWatch = new Stopwatch();
    private const int SECONDS_LIMIT = 4;
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        stopWatch.Start();
        await next.Invoke(context);
        stopWatch.Stop();

        var ts = stopWatch.Elapsed;
        if (ts.TotalSeconds > SECONDS_LIMIT)
        {
            logger.LogWarning($"Request[{context.Request.Method}] at {context.Request.Path} took {ts.TotalSeconds}s");
        }

    }
}