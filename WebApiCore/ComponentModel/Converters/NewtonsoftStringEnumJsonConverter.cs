using System;
using Newtonsoft.Json;

namespace WebApiCore.ComponentModel.Converters
{
    public class NewtonsoftStringEnumJsonConverter : JsonConverter
    {
        private static readonly Type _stringEnumType = typeof(StringEnum<>);

        public override bool CanConvert(Type objectType)
        {
            try
            {
                var baseType = _stringEnumType.MakeGenericType(objectType);
                return objectType.IsSubclassOf(baseType);
            }
            catch
            {
                return false;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var baseType = _stringEnumType.MakeGenericType(objectType);
            return baseType.GetMethod("Parse").Invoke(null, new[] { (string)reader.Value });
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
