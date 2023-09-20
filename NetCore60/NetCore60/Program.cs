using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using NetCore60.Controllers;
using NetCore60.Models;
using NetCore60.Services;
using NetCore60.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);
//// 增加请求体大小限制
//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.Limits.MaxRequestBodySize = 500 * 1024 * 1024; // 设置新的MaxRequestBodySize
//});
//builder.Services.Configure<IISServerOptions>(serverOptions =>
//{
//    serverOptions.MaxRequestBodySize = 500 * 1024 * 1024; // 设置新的MaxRequestBodySize
//});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1 * 1024 * 1024; // 设置新的MultipartBodyLengthLimit 大小限制单位是字节（Bytes） 1KB就是1024 
});


// 添加配置文件（appsettings.json）作为配置源
//builder.Configuration.AddJsonFile("appsettings.json");



// 允许跨域请求，包括本地主机
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        //builder.WithOrigins("http://localhost")
        //       .AllowAnyHeader()
        //       .AllowAnyMethod();
        // builder.WithOrigins("http://localhost:7129")
        //.AllowAnyHeader()
        //.AllowAnyMethod();
        //builder
        //     .AllowAnyOrigin()
        //     .AllowAnyHeader()
        //     .AllowAnyMethod()
        //     .WithExposedHeaders("Content-Disposition");

        builder.SetIsOriginAllowed(Origin =>
        {
            string host = new Uri(Origin).Host;

            //Console.WriteLine("Origin Host: " + host);
            return host == "localhost";
        });
    });
});


// 注册自定义中间件
//builder.Services.AddScoped<IDatabaseService, RNDatingService>();// 注册你的服务
builder.Services.AddScoped<RNDatingService>(); // 替换为你的服务类的名称

// 添加OnlineUsersService作为服务
builder.Services.AddSingleton<OnlineUsersService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            });

// ...
//builder.Services.AddSwaggerGen();
//配置第一个控制器的 Swagger
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });
//    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "TestAPI.xml")); // XML 注释文件路径

//    c.DocumentFilter<ControllerNameFilter>(new[] { "User" });

//});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("G_Test", new OpenApiInfo { Title = "TestAPI V1", Version = "G_Test" });
    c.SwaggerDoc("G_User", new OpenApiInfo { Title = "Users API", Version = "G_User" });

    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "NetCore60.xml")); // XML 注释文件路径


    // Get and display Swashbuckle version
    //var swaggerGenVersion = typeof(SwaggerGenerator).Assembly.GetName().Version;
    //Console.WriteLine($"Swashbuckle version: {swaggerGenVersion}");
    //c.DocumentFilter<ControllerNameFilter>("User"); // 将控制器名称传递给过滤器

    c.SchemaFilter<DateOnlySchemaFilter>(); // 自定义日期字段的显示方式
    c.SchemaFilter<RemoveCreatedAtPropertySchemaFilter>();

    //// 启用 XML 注释，并指定 XML 文件的路径
    //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    //c.IncludeXmlComments(xmlPath);
});
var app = builder.Build();

app.UseMiddleware<RequestLoggingMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<SwaggerUIMiddleware>();
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/G_User/swagger.json", "UsersAPI");//http
        c.SwaggerEndpoint("/swagger/G_Test/swagger.json", "TestAPI");
        c.RoutePrefix = "api";

        //c.RoutePrefix = "Test";

    });
}
// 启用CORS中间件
app.UseCors("AllowFrontend");
//app.MapGet("/", () => "DB不存在!");

//app.MapGet("/", () =>
//{
//    // Open the URL in the default browser
//    Process.Start(new ProcessStartInfo("https://localhost:7129/user/index.html") { UseShellExecute = true });

//    return "Hello, World!";
//});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//// 启用文件上传路由


//app.MapGet("/api/online-users", (OnlineUsersService onlineUsersService) =>
//{
//    int count = onlineUsersService.GetOnlineUserCount();
//    return new { Count = count };
//});
//app.Use(async (context, next) =>
//{
//    try
//    {
//        await next(); // 执行下一个中间件或请求处理程序
//    }
//    catch (Exception ex)
//    {
//        context.Response.StatusCode = 500; // 设置适当的状态码
//        await context.Response.WriteAsync(ex.Message);
//    }
//});
//app.MapPost("/upload-file",async (HttpContext context) =>
//{
//    context.Request.EnableBuffering();
//    try
//    {
//        var file = context.Request.Form.Files.FirstOrDefault();
//        // 在这里执行文件上传逻辑
//        await context.Response.WriteAsync("File uploaded successfully");
//    }
//    catch (Exception ex)
//    {
//        context.Response.StatusCode = 500; // 设置适当的状态码
//        await context.Response.WriteAsync(ex.Message);
//        // 在异常处理之后，不再返回响应，而是抛出异常以被上面的中间件捕获
//        //throw;
//    }
//});


app.Run();

