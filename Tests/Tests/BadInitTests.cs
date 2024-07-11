using FluentAssertions;
using Fody;
using Tests.Stuff;
using Xunit.Abstractions;

namespace Tests;

public class BadInitTests(ITestOutputHelper outputHelper) : TestsBase(outputHelper)
{
    [Fact]
    public void BadAttrUsage_ThrowsWeavingEx()
    {
        Assert.Throws<WeavingException>(() => new FodyTestResultInitializer<TestAssembly.BadAttrUsage.BadAttrUsage.Class>())
            .Message.Should().Be("'TargetAttribute' is not compatible with the attribute targets of 'SourceAttribute'.");
    }

    [Fact]
    public void BadPropDefaultValue_ThrowsWeavingEx()
    {
        Assert.Throws<WeavingException>(() => new FodyTestResultInitializer<TestAssembly.BadPropDefaultValue.BadPropDefaultValue.Class>())
            .Message.Should().Be("'PropertyDefaultValueAttribute(Int32)' does not match its property 'String Prop'.");
    }
}
