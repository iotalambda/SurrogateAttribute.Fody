//#define ENABLE_THIS

using System;

namespace TestAssembly;

public static class Multiple
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Source1Attribute : Attribute
#if ENABLE_THIS || ENABLE_ALL
        , SurrogateAttribute.ISurrogateAttribute
#endif
    {
        public Attribute[] TargetAttributes => [
            new Source1Target1Attribute { Source1Target1Prop = Values.Source1Target1Prop },
            new Source1Target2Attribute { Source1Target2Prop1 = Values.Source1Target2Prop1, Source1Target2Prop2 = Values.Source1Target2Prop2 },
            new Source1Target3Attribute { Source1Target3Prop = Values.Source1Target3Prop },
        ];
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class Source2Attribute : Attribute
#if ENABLE_THIS || ENABLE_ALL
        , SurrogateAttribute.ISurrogateAttribute
#endif
    {
        public Attribute[] TargetAttributes => [
            new Source2Target1Attribute { Source2Target1Prop = Values.Source2Target1Prop },
            new Source2Target2Attribute { Source2Target2Prop = Values.Source2Target2Prop },
        ];
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class Source1Target1Attribute : Attribute
    {
        public string Source1Target1Prop { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class Source1Target2Attribute : Attribute
    {
        public string Source1Target2Prop1 { get; set; }
        public string Source1Target2Prop2 { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class Source1Target3Attribute : Attribute
    {
        public string Source1Target3Prop { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class Source2Target1Attribute : Attribute
    {
        public string Source2Target1Prop { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class Source2Target2Attribute : Attribute
    {
        public string Source2Target2Prop { get; set; }
    }

    [Source1, Source2]
    public class Class;

    public static class Values
    {
        public const string Source1Target1Prop = "11";
        public const string Source1Target2Prop1 = "121";
        public const string Source1Target2Prop2 = "122";
        public const string Source1Target3Prop = "13";
        public const string Source2Target1Prop = "21";
        public const string Source2Target2Prop = "22";
    }
}
