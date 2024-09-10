using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NetCore60.Services;
using System.Text;
using System.Threading.Tasks;

public class SwaggerUIMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SwaggerUIMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var databaseService = scope.ServiceProvider.GetRequiredService<RNDatingService>();

                bool isDatabaseConnected = databaseService.CheckDatabaseConnection();

                if (!isDatabaseConnected)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    byte[] data = Encoding.UTF8.GetBytes("伺服器已關閉,請聯繫管理員Louis");
                    context.Response.ContentType = "text/plain; charset=utf-8";
                    await context.Response.Body.WriteAsync(data, 0, data.Length);
                    return;
                }
            }
        }
        await _next(context);
    }
}
