using SurrogateAttribute;
using System;

namespace TestAssembly.RefLevel1;

public static class RefLevel1
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SourceAttribute : Attribute, ISurrogateAttribute
    {
        public Attribute[] TargetAttributes => [
            new TargetAttribute(typeof(MyType)),
        ];
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TargetAttribute(Type typeArg) : Attribute
    {
        public Type TypeArg => typeArg;
    }

    [Source]
    [RefLevel2.Pattern.RefLevel2.Source]
    public class Class;

    public class MyType;

    public static class Values
    {
        public static readonly Type TypeArg = typeof(MyType);
    }
}
