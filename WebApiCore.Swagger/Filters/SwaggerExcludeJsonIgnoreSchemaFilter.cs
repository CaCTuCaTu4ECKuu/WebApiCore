using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApiCore.Swagger.Filters
{
    /// <summary>
    /// Remove properties marked with <see cref="JsonIgnoreAttribute"/> from schema
    /// </summary>
    public class SwaggerExcludeJsonIgnoreSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || context == null)
                return;

            var excludedProperties = context.Type.GetProperties()
                .Where(t => t.GetCustomAttribute(typeof(JsonIgnoreAttribute), true) != null);

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
