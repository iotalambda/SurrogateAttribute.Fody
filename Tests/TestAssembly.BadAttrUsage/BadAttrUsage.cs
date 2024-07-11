using SurrogateAttribute;
using System;

namespace TestAssembly.BadAttrUsage;

public static class BadAttrUsage
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SourceAttribute : Attribute, ISurrogateAttribute
    {
        public Attribute[] TargetAttributes => [new TargetAttribute()];
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class TargetAttribute : Attribute;

    [Source]
    public class Class;
}
