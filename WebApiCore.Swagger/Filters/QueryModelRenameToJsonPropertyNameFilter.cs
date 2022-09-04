using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApiCore.Swagger.Filters
{
    /// <summary>
    /// Rename schema properties accordingly to <see cref="JsonPropertyAttribute"/>
    /// </summary>
    public class QueryModelRenameToJsonPropertyNameFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            foreach (var param in context.ApiDescription.ParameterDescriptions.Where(d => d.Source.Id == "Query"))
            {
                var toRename = ((DefaultModelMetadata)param.ModelMetadata)
                   .Attributes.PropertyAttributes?.Any(x => x is JsonPropertyAttribute);

                if (toRename == true)
                {
                    var opParam = operation.Parameters.SingleOrDefault(p => p.Name == param.Name);
                    if (opParam != null)
                    {
                        JsonPropertyAttribute attr = (JsonPropertyAttribute)((DefaultModelMetadata)param.ModelMetadata)
                           .Attributes.PropertyAttributes.First(x => x is JsonPropertyAttribute);

                        opParam.Name = attr.PropertyName;
                    }
                }
            };
        }
    }
}
