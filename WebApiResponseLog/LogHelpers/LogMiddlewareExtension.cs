using Microsoft.AspNetCore.Builder;

namespace WebApiResponseLog.LogHelpers
{
    public static class LogMiddlewareExtension
    {
        public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogMiddleware>();
        }
    }
}
