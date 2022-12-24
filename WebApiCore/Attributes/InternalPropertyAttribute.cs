using System;

namespace WebApiCore.Attributes
{
    /// <summary>
    /// Marks property to indicate that it should be concealed from public vision
    /// </summary>
    [AttributeUsage(validOn: AttributeTargets.Property)]
    public class InternalPropertyAttribute : Attribute
    {
        public InternalPropertyAttribute()
        {

        }
    }
}
