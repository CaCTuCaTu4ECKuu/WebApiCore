using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace WebApiCore.ComponentModel.Converters
{
    public class StringEnumTypeConverter : TypeConverter
    {
        private static readonly Type _stringEnumType = typeof(StringEnum<>);

        private readonly Type _dstType;

        public StringEnumTypeConverter(Type type)
        {
            if (_stringEnumType.MakeGenericType(type).IsAssignableFrom(type))
                _dstType = type;
            else
                throw new InvalidCastException();
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                MethodInfo method = _dstType.BaseType.GetMethod("TryParse");
                object[] parameters = new object[] { value, null };
                var parseResult = (bool)method.Invoke(null, parameters);
                if (parseResult)
                    return parameters[1];
                else
                    throw new InvalidCastException();
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
                return value.ToString();

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

