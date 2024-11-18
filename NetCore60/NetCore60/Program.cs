using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NetCore60.Controllers;
using NetCore60.Models;
using NetCore60.Services;
using NetCore60.Swagger;
using NetCore60.Utilities;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;




var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["Database:ConnectionString"];

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<RNDatingService>();
builder.Services.AddScoped<UsersService>();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1 * 1024 * 1024; // 设置新的MultipartBodyLengthLimit 大小限制单位是字节（Bytes） 1KB就是1024 
    //options.ValueLengthLimit = int.MaxValue; // Maximum size of a form or uploaded file, adjust as needed
    //options.MultipartBodyLengthLimit = long.MaxValue; // Maximum length of multipart body data (e.g., file uploads), adjust as needed
    //options.MultipartHeadersLengthLimit = int.MaxValue; // Maximum length of multipart headers, adjust as needed

});

//int keyLengthInBytes = 32; // 生成32字节的密钥（256位）
//string secretKey = JsonWebTokenService.GenerateSecretKey(keyLengthInBytes);
//string generateJwtToken = JsonWebTokenService.GenerateJwtToken(5);
//int getUserIdFromToken = JsonWebTokenService.TryGetUserIdFromJwtToken(generateJwtToken);

//Console.WriteLine(secretKey);

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
    //options.AddPolicy("AllowLocalhost", builder =>
    //{
    //    builder.SetIsOriginAllowed(Origin =>
    //    {
    //        string host = new Uri(Origin).Host;

    //        //Console.WriteLine("Origin Host: " + host);
    //        return host == "localhost";
    //    });
    //});
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null; // 保持屬性名稱不變
                options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            });
builder.Services.AddSwaggerGen(options =>
{
    //產生Swagger json
    options.SwaggerDoc("G_Test", new OpenApiInfo { Title = "TestAPI V1", Version = "1.0" });
    options.SwaggerDoc("G_User", new OpenApiInfo { Title = "Users API", Version = "1.0" });
    options.SwaggerDoc("G_Stocks", new OpenApiInfo { Title = "StockInformation API", Version = "2.0" });
    options.SwaggerDoc("SwaggerGroupGuitarTutorial", new OpenApiInfo { Title = "GuitarTutorialAPI", Version = "1.0" });
    options.AddSecurityDefinition("Token", new OpenApiSecurityScheme
    {
        Description = "直接輸入 JWT Token，不需加上 'Bearer ' 前綴。",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Token"
            }
        },
        Array.Empty<string>()
    }
    });

    //options.DocumentFilter<DisableSchemaGenerationFilter>();
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "NetCore60.xml")); // XML 注释文件路径
    options.SchemaFilter<DateOnlySchemaFilter>(); // 自定义日期字段的显示方式
    options.SchemaFilter<RemoveCreatedAtPropertySchemaFilter>();

    // Get and display Swashbuckle version
    //var swaggerGenVersion = typeof(SwaggerGenerator).Assembly.GetName().Version;
    //Console.WriteLine($"Swashbuckle version: {swaggerGenVersion}");
    //options.DocumentFilter<ControllerNameFilter>("User"); // 将控制器名称传递给过滤器



    //// 启用 XML 注释，并指定 XML 文件的路径
    //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    //options.IncludeXmlComments(xmlPath);
});
var app = builder.Build();
app.UseMiddleware<RequestLoggingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Shows detailed error pages in Development
    app.UseMiddleware<SwaggerUIMiddleware>();
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/G_User/swagger.json", "UsersAPI");//http
        //options.SwaggerEndpoint("/swagger/G_Test/swagger.json", "TestAPI");
        c.SwaggerEndpoint("/swagger/G_Stocks/swagger.json", "StockAPI");
        c.SwaggerEndpoint("/swagger/SwaggerGroupGuitarTutorial/swagger.json", "GuitarTutorialAPI");
        c.RoutePrefix = "api";
        // 控制示例的最大显示大小
        c.DefaultModelExpandDepth(2); // 设置展开的深度
        c.DefaultModelRendering(ModelRendering.Model); // 使用 Model 渲染
        c.DefaultModelsExpandDepth(-1); // 设置不展开所有模型
        c.DisplayRequestDuration(); // 显示请求持续时间
        //options.RoutePrefix = "Test";

    });
}
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

