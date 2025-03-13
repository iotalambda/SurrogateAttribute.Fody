//#define ENABLE_THIS

using SurrogateAttribute;
using System;

namespace TestAssembly;

public static class PropSources
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SourceAttribute(string stringPropFromCtorArg, Type typePropFromCtorArg, string[] arrayStringPropFromCtorArg) : Attribute
#if ENABLE_THIS || ENABLE_ALL
        , ISurrogateAttribute
#endif
    {
        public string StringPropFromNamedArg { get; set; }

        [PropertyDefaultValue(Values.StringPropFromDefault)]
        public string StringPropFromDefault { get; set; }

        public string[] ArrayStringPropFromNamedArg { get; set; }

        [PropertyDefaultValue(["D", "E", "F"])]
        public string[] ArrayStringPropFromDefault { get; set; }

        public Type TypePropFromNamedArg { get; set; }

        [PropertyDefaultValue(typeof(TypePropFromDefault))]
        public Type TypePropFromDefault { get; set; }

        public Attribute[] TargetAttributes =>
            [new TargetAttribute
            {
                StringPropFromCtorArg = stringPropFromCtorArg,
                StringPropFromNamedArg = StringPropFromNamedArg,
                StringPropFromDefault = StringPropFromDefault,
                StringPropFromConst = Values.StringPropFromConst,
                ArrayStringPropFromCtorArg = arrayStringPropFromCtorArg,
                ArrayStringPropFromNamedArg = ArrayStringPropFromNamedArg,
                ArrayStringPropFromDefault = ArrayStringPropFromDefault,
                TypePropFromCtorArg = typePropFromCtorArg,
                TypePropFromNamedArg = TypePropFromNamedArg,
                TypePropFromDefault = TypePropFromDefault,
                TypePropFromConst = typeof(TypePropFromConst),
            }];
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TargetAttribute : Attribute
    {
        public string StringPropFromCtorArg { get; set; }
        public string StringPropFromNamedArg { get; set; }
        public string StringPropFromDefault { get; set; }
        public string StringPropFromConst { get; set; }
        public string[] ArrayStringPropFromCtorArg { get; set; }
        public string[] ArrayStringPropFromNamedArg { get; set; }
        public string[] ArrayStringPropFromDefault { get; set; }
        public Type TypePropFromCtorArg { get; set; }
        public Type TypePropFromNamedArg { get; set; }
        public Type TypePropFromDefault { get; set; }
        public Type TypePropFromConst { get; set; }
    }

    [Source(
        Values.StringPropFromCtorArg,
        typeof(TypePropFromCtorArg),
        ["X", "Y", "Z"],
        StringPropFromNamedArg = Values.StringPropFromNamedArg,
        ArrayStringPropFromNamedArg = ["A", "B", "C"],
        TypePropFromNamedArg = typeof(TypePropFromNamedArg)
    )]
    public class Class;

    public static class Values
    {
        public const string StringPropFromCtorArg = "0";
        public const string StringPropFromNamedArg = "1";
        public const string StringPropFromDefault = "2";
        public const string StringPropFromConst = "3";
        public static readonly string[] ArrayStringPropFromCtorArg = ["X", "Y", "Z"];
        public static readonly string[] ArrayStringPropFromNamedArg = ["A", "B", "C"];
        public static readonly string[] ArrayStringPropFromDefault = ["D", "E", "F"];
        public static readonly Type TypePropFromCtorArg = typeof(TypePropFromCtorArg);
        public static readonly Type TypePropFromNamedArg = typeof(TypePropFromNamedArg);
        public static readonly Type TypePropFromDefault = typeof(TypePropFromDefault);
        public static readonly Type TypePropFromConst = typeof(TypePropFromConst);
    }

    public class TypePropFromCtorArg;
    public class TypePropFromNamedArg;
    public class TypePropFromDefault;
    public class TypePropFromConst;
}
