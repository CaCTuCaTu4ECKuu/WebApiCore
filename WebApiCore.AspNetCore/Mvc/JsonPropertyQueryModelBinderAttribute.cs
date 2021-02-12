using Microsoft.AspNetCore.Mvc;

namespace WebApiCore.AspNetCore.Mvc
{
    public class JsonPropertyQueryModelBinderAttribute : ModelBinderAttribute
    {
        public JsonPropertyQueryModelBinderAttribute() : base(typeof(JsonPropertyQueryModelBinder))
        { }
    }
}
