using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetCore60.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string? _connectionString;
    public RequestLoggingMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _connectionString = configuration.GetConnectionString("RNDatingDBConnection");

    }

    public async Task InvokeAsync(HttpContext _HttpContext)
    {
        using (var context = new RndatingDbContext(_connectionString)) 
        { 

            try
            {
                string[] targetPaths = { "/api/index.html", "/swagger/G_User/swagger.json" };

                if (targetPaths.Any(path => _HttpContext.Request.Path.ToString().Contains(path)))
                {
                    await _next(_HttpContext);
                    return;
                }
                var requestLog = new RequestLog()
                {
                    Path = _HttpContext.Request.Path,
                    Method = _HttpContext.Request.Method,
                    ClientIp = _HttpContext.Connection.RemoteIpAddress?.ToString() ?? "",
                    Timestamp = DateTime.UtcNow
                };

                // 将 requestLog 添加到 context.RequestLogs 中
                context.RequestLogs.Add(requestLog);

                // 保存更改到数据库
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var test = new RndatingDbContext(_connectionString);
                var sqlExceptionMessage = "" + ex.InnerException?.Message;
                // 将异常消息保存到 RecordLogTable 中
                test.RecordLogTables.Add(new RecordLogTable { DataText = sqlExceptionMessage });
                // 保存更改到数据库
                test.SaveChanges();
                Console.WriteLine("Error: " + sqlExceptionMessage); // 打印错误消息
                // 将异常重新抛出，继续传播异常
                throw;
            }

            // 继续处理请求
            await _next(_HttpContext);
        }
    }
}
