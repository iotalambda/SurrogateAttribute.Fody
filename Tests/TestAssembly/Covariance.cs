//#define ENABLE_THIS

using System;

namespace TestAssembly;
public static class Covariance
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SourceAttribute : Attribute
#if ENABLE_THIS || ENABLE_ALL
        , SurrogateAttribute.ISurrogateAttribute
#endif
    {
        public string[] ArrayStringPropFromNamedArg { get; set; }

        public Attribute[] TargetAttributes => [new TargetAttribute(ArrayStringPropFromNamedArg)];
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TargetAttribute : Attribute
    {
        public TargetAttribute(object[] arrayObjectArg)
        {
            ArrayObjectArg = arrayObjectArg;
        }

        public object[] ArrayObjectArg { get; set; }
    }

    [Source(ArrayStringPropFromNamedArg = ["A", "B", "C"])]
    public class Class;

    public static class Values
    {
        public static readonly string[] ArrayStringPropFromNamedArg = ["A", "B", "C"];
    }
}
