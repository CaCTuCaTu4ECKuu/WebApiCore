using Microsoft.AspNetCore.Mvc;

namespace WebApiCore.AspNetCore.Mvc
{
    /// <summary>
    /// Attribute for <see cref="JsonPropertyQueryModelBinder"/> that allows to bind properties from query (when it's doesn't by default)
    /// </summary>
    public class JsonPropertyQueryModelBinderAttribute : ModelBinderAttribute
    {
        public JsonPropertyQueryModelBinderAttribute() : base(typeof(JsonPropertyQueryModelBinder))
        { }
    }
}
