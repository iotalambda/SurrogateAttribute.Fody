using Fody;
using Shared;
using Shouldly;
using Xunit.Abstractions;

namespace Tests;

public class BadInitTests(ITestOutputHelper outputHelper) : TestsBase(outputHelper)
{
    [Fact]
    public void BadAttrUsage_ThrowsWeavingEx()
    {
        Assert.Throws<WeavingException>(() => new FodyTestResultInitializer<TestAssembly.BadAttrUsage.BadAttrUsage.Class>().Initialize())
            .Message.ShouldBe("'TargetAttribute' is not compatible with the attribute targets of 'SourceAttribute'.");
    }

    [Fact]
    public void BadPropDefaultValue_ThrowsWeavingEx()
    {
        Assert.Throws<WeavingException>(() => new FodyTestResultInitializer<TestAssembly.BadPropDefaultValue.BadPropDefaultValue.Class>().Initialize())
            .Message.ShouldBe("'PropertyDefaultValueAttribute(Int32)' does not match its property 'String Prop'.");
    }

    [Fact]
    public void BadCtorArgMapping_ThrowsWeavingEx()
    {
        Assert.Throws<WeavingException>(() => new FodyTestResultInitializer<TestAssembly.BadCtorArgMapping.BadCtorArgMapping.Class>().Initialize())
            .Message.ShouldBe($"Cannot use complex operations when assigning a value to 'vField' in the constructor of 'SourceAttribute'.");
    }
}
