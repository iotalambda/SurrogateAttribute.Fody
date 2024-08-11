using SurrogateAttribute;
using System;

namespace TestAssembly.BadCtorArgMapping;

public static class BadCtorArgMapping
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SourceAttribute : Attribute, ISurrogateAttribute
    {
        int vField;

        public SourceAttribute(int v)
        {
            vField = v + 1;
        }

        public Attribute[] TargetAttributes => [new TargetAttribute { V = vField }];
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TargetAttribute : Attribute
    {
        public int V { get; set; }
    }

    [Source(100)]
    public class Class;
}
