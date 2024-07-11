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
        public PropertyDefaultValueAttribute(bool value) => Value = value;

        /// <inheritdoc cref="PropertyDefaultValueAttribute(bool)" />
        public PropertyDefaultValueAttribute(byte value) => Value = value;

        /// <inheritdoc cref="PropertyDefaultValueAttribute(bool)" />
        public PropertyDefaultValueAttribute(char value) => Value = value;

        /// <inheritdoc cref="PropertyDefaultValueAttribute(bool)" />
        public PropertyDefaultValueAttribute(double value) => Value = value;

        /// <inheritdoc cref="PropertyDefaultValueAttribute(bool)" />
        public PropertyDefaultValueAttribute(float value) => Value = value;

        /// <inheritdoc cref="PropertyDefaultValueAttribute(bool)" />
        public PropertyDefaultValueAttribute(int value) => Value = value;

        /// <inheritdoc cref="PropertyDefaultValueAttribute(bool)" />
        public PropertyDefaultValueAttribute(long value) => Value = value;

        /// <inheritdoc cref="PropertyDefaultValueAttribute(bool)" />
        public PropertyDefaultValueAttribute(string value) => Value = value;

        /// <inheritdoc cref="PropertyDefaultValueAttribute(bool)" />
        public PropertyDefaultValueAttribute(Type value) => Value = value;
    }
}
