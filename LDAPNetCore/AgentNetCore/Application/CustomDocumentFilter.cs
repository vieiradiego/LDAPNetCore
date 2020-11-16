using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace AgentNetCore.Application
{
    public class CustomDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var oap = new OpenApiPaths();
            foreach (var p in swaggerDoc.Paths)
            {
                oap.Add(p.Key.Replace("v{version}", swaggerDoc.Info.Version), p.Value);
            }
            swaggerDoc.Paths = oap;
        }
    }
}
