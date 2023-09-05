
using Microsoft.OpenApi.Models;
using NetCore60.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
namespace NetCore60.Swagger
{

    public class RemoveCreatedAtPropertySchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            // 在这里检查模型名称和要移除的属性名称
            Type modelType = context.Type;
            if (modelType.FullName != null && modelType.FullName.Contains("NetCore60.Models"))
            //VUsersDetail
            {
                if (model.Properties.ContainsKey("createdAt"))
                {
                    model.Properties.Remove("createdAt");
                    model.Properties.Remove("updatedAt");
                }
                if (modelType.FullName.Contains("VUsersDetail"))
                {
                    model.Properties.Remove("udUserId");
                }

            }

        }
    }

}

