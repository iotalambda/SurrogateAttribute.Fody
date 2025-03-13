
using Shouldly;

namespace Tests2;

public class DependencyTests
{
    [Fact]
    public void RefLevelsFrom0To012_Ok()
    {
        var type = typeof(Stuff.Types.Class);
        type.ShouldBeDecoratedWith<Stuff.Types.TargetAttribute>();
        type.ShouldBeDecoratedWith<TestAssembly.RefLevel1.RefLevel1.TargetAttribute>();
        type.ShouldBeDecoratedWith<TestAssembly.RefLevel2.Pattern.RefLevel2.TargetAttribute>();
    }

    [Fact]
    public void RefLevelsFrom1To12_Ok()
    {
        var type = typeof(TestAssembly.RefLevel1.RefLevel1.Class);
        type.ShouldBeDecoratedWith<TestAssembly.RefLevel1.RefLevel1.TargetAttribute>();
        type.ShouldBeDecoratedWith<TestAssembly.RefLevel2.Pattern.RefLevel2.TargetAttribute>();
    }

    [Fact]
    public void RefLevelsFrom2To2_Ok()
    {
        var type = typeof(TestAssembly.RefLevel2.Pattern.RefLevel2.Class);
        type.ShouldBeDecoratedWith<TestAssembly.RefLevel2.Pattern.RefLevel2.TargetAttribute>();
    }
}