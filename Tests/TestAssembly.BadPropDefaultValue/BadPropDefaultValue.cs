using SurrogateAttribute;
using System;

namespace TestAssembly.BadPropDefaultValue;

public static class BadPropDefaultValue
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SourceAttribute : Attribute, ISurrogateAttribute
    {
        [PropertyDefaultValue(123)]
        public string Prop { get; set; }

        public Attribute[] TargetAttributes => [new TargetAttribute
        {
            Prop = Prop,
        }];
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TargetAttribute : Attribute
    {
        public string Prop { get; set; }
    }

    [Source]
    public class Class;
}
