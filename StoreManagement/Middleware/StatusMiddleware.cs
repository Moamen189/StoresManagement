using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace StoreManagement.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class StatusMiddleware
    {
        private readonly RequestDelegate _next;

        public StatusMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            DateTime requestTime = DateTime.Now;
            var result = _next(httpContext);
            DateTime responseTime = DateTime.Now;
            TimeSpan processTime = requestTime - requestTime;
            Console.WriteLine("Process Duration=" + processTime.TotalMicroseconds + "ms")

            return result;

            
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class StatusMiddlewareExtensions
    {
        public static IApplicationBuilder UseStatusMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<StatusMiddleware>();
        }
    }
}
