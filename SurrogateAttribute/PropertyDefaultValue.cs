using System;

namespace SurrogateAttribute
{
    /// <summary>
    /// Sets the default value for a property of a surrogate attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyDefaultValueAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Sets the default value for a property of a surrogate attribute.
        /// </summary>
        public PropertyDefaultValueAttribute(object value)
        {
            Value = value;
        }
    }
}
