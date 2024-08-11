//#define ENABLE_THIS

using System;

namespace TestAssembly;

public static class CtorArgMapping
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SourceAttribute : Attribute
#if ENABLE_THIS || ENABLE_ALL
        , SurrogateAttribute.ISurrogateAttribute
#endif
    {
        public string PropToArg { get; set; }

        public Attribute[] TargetAttributes =>
            [new TargetAttribute(
                PropToArg,
                argToArgField)
            {
                ArgToProp = argToPropField
            }];

        string argToPropField;
        string argToArgField;

        public SourceAttribute(string argToProp, string argToArg)
        {
            argToArgField = argToArg;
            argToPropField = argToProp;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class TargetAttribute : Attribute
    {
        public string PropToArg { get; }
        public string ArgToProp { get; set; }
        public string ArgToArg { get; }

        public TargetAttribute(string propToArg, string argToArg)
        {
            PropToArg = propToArg;
            ArgToArg = argToArg;
        }
    }

    public class Class
    {
        [Source(Values.ArgToProp, Values.ArgToArg, PropToArg = Values.PropToArg)]
        public string ClassProp { get; set; }
    }

    public static class Values
    {
        public const string PropToArg = "1";
        public const string ArgToProp = "2";
        public const string ArgToArg = "3";
    }
}
