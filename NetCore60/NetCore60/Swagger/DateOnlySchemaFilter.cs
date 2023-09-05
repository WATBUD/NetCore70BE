
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
namespace NetCore60.Swagger
{

    public class DateOnlySchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // 检查是否是 System.DateOnly 类型的属性
            if (context.Type == typeof(System.DateOnly) || context.Type == typeof(System.DateOnly?))
            {
                // 将属性类型更改为字符串
                schema.Type = "string";
                schema.Format = "date"; // 指定日期格式
            }
        }
    }
}

