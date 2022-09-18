using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiCore.AspNetCore.ActionFilters;

namespace WebApiCore.Swagger.Filters
{
    public class AccessTokenPropertyFilter<TTokenAttribute> : IOperationFilter
        where TTokenAttribute : RequireAccessTokenAttribute
    {
        public string ParameterName { get; set; }

        public AccessTokenPropertyFilter(string parameterName = "access_token")
        {
            ParameterName = parameterName;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            var hasAuthAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<TTokenAttribute>()
                .Any();

            var isGet = context.MethodInfo.GetCustomAttributes(true)
                .OfType<HttpGetAttribute>()
                .Any();

            if (hasAuthAttribute)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = ParameterName,
                    In = isGet
                        ? ParameterLocation.Query
                        : ParameterLocation.Header,
                    Description = "Access token",
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    },
                    AllowReserved = true
                });
            }
        }
    }
}
