using NetCore60.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Text;

public class SwaggerUIMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public SwaggerUIMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            var databaseService = new RNDatingService(_configuration);
            bool isDatabaseConnected = databaseService.CheckDatabaseConnection();

            // Check if the database is not connected and the request path is not Swagger UI
            if (!isDatabaseConnected)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                // This code will run when entering Swagger UI
                // Perform your custom actions here
                // Specify UTF-8 encoding
                byte[] data = Encoding.UTF8.GetBytes("伺服器已關閉,請聯繫管理員Louis");
                context.Response.ContentType = "text/plain; charset=utf-8";
                await context.Response.Body.WriteAsync(data, 0, data.Length);
                return;
            }
        }
        await _next(context);
    }
}
