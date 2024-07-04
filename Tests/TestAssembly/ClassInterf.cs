//#define ENABLE_THIS

using System;

namespace TestAssembly;

public static class ClassInterf
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SourceClassAttribute : Attribute
#if ENABLE_THIS || ENABLE_ALL
        , SurrogateAttribute.ISurrogateAttribute
#endif
    {
        public Attribute[] TargetAttributes => [new TargetClassAttribute()];
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TargetClassAttribute : Attribute { }

    [SourceClass]
    public class Class : IInterf { }

    [AttributeUsage(AttributeTargets.Interface)]
    public class SourceInterfAttribute : Attribute
#if ENABLE_THIS || ENABLE_ALL
        , SurrogateAttribute.ISurrogateAttribute
#endif
    {
        public Attribute[] TargetAttributes => [new TargetInterfAttribute()];
    }

    [AttributeUsage(AttributeTargets.Interface)]
    public class TargetInterfAttribute : Attribute { }

    [SourceInterf]
    public interface IInterf { }
}
