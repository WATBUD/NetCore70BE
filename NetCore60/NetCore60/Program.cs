using Microsoft.OpenApi.Models;
using NetCore60.Controllers;
using NetCore60.Services;
using NetCore60.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
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
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    //c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });

    c.SchemaFilter<DateOnlySchemaFilter>(); // 自定义日期字段的显示方式
    c.SchemaFilter<RemoveCreatedAtPropertySchemaFilter>();
    // 启用 XML 注释，并指定 XML 文件的路径
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo
//    {
//        Title = "Your API Name",
//        Version = "v1",
//        Description = "Description of your API",
//        Contact = new OpenApiContact
//        {
//            Name = "Your Name",
//            Email = "your@email.com"
//        }
//    });
//});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<SwaggerUIMiddleware>();
    app.UseSwagger();
    app.UseSwaggerUI();

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



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

