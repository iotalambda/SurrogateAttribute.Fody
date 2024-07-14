using FluentAssertions;
using SurrogateAttribute;

namespace Tests2;

public class DependencyTests
{
    [Fact]
    public void RefLevelsFrom0To012_Ok()
    {
        typeof(DependencyTests_Types.Class).Should()
            .BeDecoratedWith<DependencyTests_Types.TargetAttribute>()
            .And.BeDecoratedWith<TestAssembly.RefLevel1.RefLevel1.TargetAttribute>()
            .And.BeDecoratedWith<TestAssembly.RefLevel2.Pattern.RefLevel2.TargetAttribute>();
    }

    [Fact]
    public void RefLevelsFrom1To12_Ok()
    {
        typeof(TestAssembly.RefLevel1.RefLevel1.Class).Should()
            .BeDecoratedWith<TestAssembly.RefLevel1.RefLevel1.TargetAttribute>()
            .And.BeDecoratedWith<TestAssembly.RefLevel2.Pattern.RefLevel2.TargetAttribute>();
    }

    [Fact]
    public void RefLevelsFrom2To2_Ok()
    {
        typeof(TestAssembly.RefLevel2.Pattern.RefLevel2.Class).Should()
            .BeDecoratedWith<TestAssembly.RefLevel2.Pattern.RefLevel2.TargetAttribute>();
    }
}

public class DependencyTests_Types
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
