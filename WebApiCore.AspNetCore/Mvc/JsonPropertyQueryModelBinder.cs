using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace WebApiCore.AspNetCore.Mvc
{
    public class JsonPropertyQueryModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            Dictionary<PropertyInfo, string> modelProps = new Dictionary<PropertyInfo, string>();
            foreach (var prop in bindingContext.ModelType.GetProperties())
            {
                var jp = prop.GetCustomAttributes<JsonPropertyNameAttribute>().FirstOrDefault();
                if (jp != null)
                    modelProps.Add(prop, jp.Name);
                else
                {
                    var njp = prop.GetCustomAttributes<JsonPropertyAttribute>().FirstOrDefault();
                    if (njp != null)
                        modelProps.Add(prop, njp.PropertyName);
                    else
                        modelProps.Add(prop, prop.Name);
                }
            }

            var model = Activator.CreateInstance(bindingContext.ModelType);
            foreach (var prop in modelProps)
            {
                var val = bindingContext.ValueProvider.GetValue(prop.Value);
                if (val != null && val.Length > 0)
                {
                    TypeConverter converter;
                    if (prop.Key.PropertyType.IsArray)
                    {
                        var propElementType = prop.Key.PropertyType.GetElementType();
                        converter = TypeDescriptor.GetConverter(propElementType);
                        var values = Array.CreateInstance(propElementType, val.Length);
                        bool success = true;
                        for (int i = 0; i < val.Length; i++)
                        {
                            try
                            {
                                values.SetValue(converter.ConvertFrom(val.Values.ElementAt(i)), i);
                            }
                            catch
                            {
                                success = false;
                                break;
                            }
                        }
                        if (success)
                        {
                            prop.Key.SetValue(model, values);
                            continue;
                        }
                    }
                    else
                    {
                        converter = TypeDescriptor.GetConverter(prop.Key.PropertyType);
                        try
                        {
                            prop.Key.SetValue(model, converter.ConvertFrom(val.Values.Last()));
                            continue;
                        }
                        catch
                        { }
                    }

                    bindingContext.ModelState.TryAddModelError(prop.Value, $"The value \"{prop.Value}\" is not valid.");
                }
            }

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}
