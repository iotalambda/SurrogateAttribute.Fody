//#define ENABLE_THIS

using System;

namespace TestAssembly;

public static class TargetAttrInheritance
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SourceAttribute : Attribute
#if ENABLE_THIS || ENABLE_ALL
        , SurrogateAttribute.ISurrogateAttribute
#endif
    {
        public Attribute[] TargetAttributes =>
            [new TargetAttribute
            {
                ChildProp = Values.ChildProp,
                VirtualProp = Values.VirtualProp,
                BaseProp = Values.BaseProp,
            }];
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TargetAttribute : TargetBaseAttribute
    {
        public string ChildProp { get; set; }
        public override string VirtualProp { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TargetBaseAttribute : Attribute
    {
        public virtual string VirtualProp { get; set; }
        public string BaseProp { get; set; }
    }

    [Source]
    public class Class { }

    public static class Values
    {
        public const string ChildProp = "child";
        public const string VirtualProp = "virtual";
        public const string BaseProp = "base";
    }
}
