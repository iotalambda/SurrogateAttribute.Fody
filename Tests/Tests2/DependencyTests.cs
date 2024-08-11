using FluentAssertions;

namespace Tests2;

public class DependencyTests
{
    [Fact]
    public void RefLevelsFrom0To012_Ok()
    {
        typeof(Stuff.Types.Class).Should()
            .BeDecoratedWith<Stuff.Types.TargetAttribute>()
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