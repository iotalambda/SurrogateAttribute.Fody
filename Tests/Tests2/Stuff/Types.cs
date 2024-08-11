using SurrogateAttribute;

namespace Tests2.Stuff;

public static class Types
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SourceAttribute : Attribute, ISurrogateAttribute
    {
        public Attribute[] TargetAttributes => [new TargetAttribute()];
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class TargetAttribute : Attribute;

    [Source]
    [TestAssembly.RefLevel1.RefLevel1.Source]
    [TestAssembly.RefLevel2.Pattern.RefLevel2.Source]
    public class Class;
}