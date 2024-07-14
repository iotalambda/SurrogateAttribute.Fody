using SurrogateAttribute;
using System;

namespace TestAssembly.RefLevel1;

public static class RefLevel1
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
    [RefLevel2.Pattern.RefLevel2.Source]
    public class Class;
}
