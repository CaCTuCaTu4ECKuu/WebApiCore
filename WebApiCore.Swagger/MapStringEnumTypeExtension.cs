﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

using WebApiCore.ComponentModel;

namespace WebApiCore.Swagger
{
    public static class MapStringEnumTypeExtension
    {
        public static void MapStringEnumType<TEnum>(this SwaggerGenOptions opt)
            where TEnum : StringEnum<TEnum>, IEquatable<TEnum>
        {
            opt.MapType(typeof(TEnum), () => new OpenApiSchema() { Type = "string" });
        }
    }
}
