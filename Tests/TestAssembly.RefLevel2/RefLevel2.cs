using SurrogateAttribute;
using System;

namespace TestAssembly.RefLevel2.Pattern;

public static class RefLevel2
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SourceAttribute : Attribute, ISurrogateAttribute
    {
        public Attribute[] TargetAttributes => [
            new TargetAttribute(),
        ];
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TargetAttribute : Attribute;

    [Source]
    public class Class;
}
