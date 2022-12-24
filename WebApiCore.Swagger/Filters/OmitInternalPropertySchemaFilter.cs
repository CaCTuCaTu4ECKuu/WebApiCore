using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiCore.Attributes;

namespace WebApiCore.Swagger.Filters
{
    /// <summary>
    /// Remove properties marked with InternalProperty attribute from schema
    /// </summary>
    public class OmitInternalPropertySchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || context == null || schema.Properties.Count == 0)
                return;

            var excludedProperties = context.Type.GetProperties()
                .Where(t => t.GetCustomAttribute(typeof(InternalPropertyAttribute), true) != null ||
                    t.GetCustomAttribute(typeof(Newtonsoft.Json.JsonIgnoreAttribute), true) != null);

            if (excludedProperties.Any())
            {
                foreach (var excludedProperty in excludedProperties)
                {
                    if (schema.Properties.ContainsKey(excludedProperty.Name))
                        schema.Properties.Remove(excludedProperty.Name);
                }
            }
        }
    }
}
