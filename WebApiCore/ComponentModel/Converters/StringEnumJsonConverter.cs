using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApiCore.ComponentModel.Converters
{
    public class StringEnumJsonConverter<T> : JsonConverter<StringEnum<T>>
        where T : StringEnum<T>, IEnumerable<T>, IEquatable<T>
    {
        public static readonly Type StringEnumType = typeof(StringEnum<>);

        public override StringEnum<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var baseType = StringEnumType.MakeGenericType(typeToConvert);
            return (T)baseType.GetMethod("Parse").Invoke(null, new[] { reader.GetString() });
        }

        public override void Write(Utf8JsonWriter writer, StringEnum<T> value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }

    public class StringEnumJsonConverterFactory : JsonConverterFactory
    {
        public static readonly Type StringEnumType = typeof(StringEnum<>);

        public override bool CanConvert(Type typeToConvert)
        {
            try
            {
                var baseType = StringEnumType.MakeGenericType(typeToConvert);
                return typeToConvert.IsSubclassOf(baseType);
            }
            catch
            {
                return false;
            }
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var converterType = typeof(StringEnumJsonConverter<>).MakeGenericType(typeToConvert);
            return (JsonConverter)Activator.CreateInstance(converterType);
        }
    }
}
