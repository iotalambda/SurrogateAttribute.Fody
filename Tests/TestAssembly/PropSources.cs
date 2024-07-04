//#define ENABLE_THIS

using SurrogateAttribute;
using System;

namespace TestAssembly;

public static class PropSources
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SourceAttribute : Attribute
#if ENABLE_THIS || ENABLE_ALL
        , ISurrogateAttribute
#endif
    {
        public string StringPropFromNamedArg { get; set; }
        [PropertyDefaultValue(Values.StringPropFromDefault)]
        public string StringPropFromDefault { get; set; }

        public Type TypePropFromNamedArg { get; set; }
        [PropertyDefaultValue(typeof(TypePropFromDefault))]
        public Type TypePropFromDefault { get; set; }

        public Attribute[] TargetAttributes =>
            [new TargetAttribute
            {
                StringPropFromNamedArg = StringPropFromNamedArg,
                StringPropFromDefault = StringPropFromDefault,
                StringPropFromConst = Values.StringPropFromConst,
                TypePropFromNamedArg = TypePropFromNamedArg,
                TypePropFromDefault = TypePropFromDefault,
                TypePropFromConst = typeof(TypePropFromConst),
            }];
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TargetAttribute : Attribute
    {
        public string StringPropFromNamedArg { get; set; }
        public string StringPropFromDefault { get; set; }
        public string StringPropFromConst { get; set; }
        public Type TypePropFromNamedArg { get; set; }
        public Type TypePropFromDefault { get; set; }
        public Type TypePropFromConst { get; set; }
    }

    [Source(StringPropFromNamedArg = Values.StringPropFromNamedArg, TypePropFromNamedArg = typeof(TypePropFromNamedArg))]
    public class Class;

    public static class Values
    {
        public const string StringPropFromNamedArg = "1";
        public const string StringPropFromDefault = "2";
        public const string StringPropFromConst = "3";
        public static readonly Type TypePropFromNamedArg = typeof(TypePropFromNamedArg);
        public static readonly Type TypePropFromDefault = typeof(TypePropFromDefault);
        public static readonly Type TypePropFromConst = typeof(TypePropFromConst);
    }

    public class TypePropFromNamedArg;
    public class TypePropFromDefault;
    public class TypePropFromConst;
}
