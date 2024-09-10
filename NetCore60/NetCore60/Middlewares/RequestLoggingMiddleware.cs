using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetCore60.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RequestLoggingMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            string[] targetPaths = { "/api/index.html", "/swagger/G_User/swagger.json" };

            if (targetPaths.Any(path => httpContext.Request.Path.ToString().Contains(path)))
            {
                await _next(httpContext);
                return;
            }

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<RndatingDbContext>();

                var requestLog = new RequestLog()
                {
                    Path = httpContext.Request.Path,
                    Method = httpContext.Request.Method,
                    ClientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "",
                    CreatedAt = DateTime.UtcNow,
                    BackendLanguage = "C#"
                };

                context.RequestLogs.Add(requestLog);

                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<RndatingDbContext>();
                var sqlExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                context.RecordLogTables.Add(new RecordLogTable { DataText = sqlExceptionMessage });
                context.SaveChanges();
            }

            Console.WriteLine("Error: " + ex.Message);
            throw;
        }

        await _next(httpContext);
    }
}
