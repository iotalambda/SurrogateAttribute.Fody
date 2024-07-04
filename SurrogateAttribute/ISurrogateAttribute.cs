using System;

namespace SurrogateAttribute
{
    /// <summary>
    /// Makes it possible to abstract, combine and reuse attributes. Implemented by surrogate attributes.
    /// </summary>
    public interface ISurrogateAttribute
    {
        /// <summary>
        /// Attributes that replace the surrogate attribute.
        /// </summary>
        Attribute[] TargetAttributes { get; }
    }
}
