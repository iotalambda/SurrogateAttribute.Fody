using FluentAssertions;
using SurrogateAttribute;

namespace Tests2;

public class AssemblyTests
{
    [Fact]
    public void GetTypes_ReturnsNoSurrogateAttrs()
    {
        var types = typeof(AssemblyTests).Assembly.GetTypes();
        types.Should().NotContain(t => t.Name == nameof(SourceAttribute));
        types.Should().Contain(t => t == typeof(TargetAttribute));
    }
}

class SourceAttribute : Attribute, ISurrogateAttribute
{
    public Attribute[] TargetAttributes => [new TargetAttribute()];
}

class TargetAttribute : Attribute;