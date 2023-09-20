using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class DisableSchemaGenerationFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // 禁用生成 Schemas
        swaggerDoc.Components.Schemas.Clear();
    }
}
