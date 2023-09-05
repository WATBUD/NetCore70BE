using Microsoft.OpenApi.Models;
using NetCore60.Controllers;
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

// Add services to the container.
builder.Services.AddControllers();

//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
//    });




// 注册自定义中间件

//builder.Services.AddScoped<IDatabaseService, RNDatingService>();// 注册你的服务
builder.Services.AddScoped<RNDatingService>(); // 替换为你的服务类的名称



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<SwaggerUIMiddleware>();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/G_User/swagger.json", "UsersAPI");
        c.SwaggerEndpoint("/swagger/G_Test/swagger.json", "TestAPI");
        c.RoutePrefix = "api";

        //c.RoutePrefix = "Test";

    });
    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("/swagger/G_Test/swagger.json", "TestAPI");
    //    c.RoutePrefix = "Test";
    //});
    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1");
    //});
    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
    //    c.RoutePrefix = "222";
    //});
}
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

app.Run();

