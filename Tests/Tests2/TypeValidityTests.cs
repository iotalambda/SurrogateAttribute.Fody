using FluentAssertions;
using SurrogateAttribute;

namespace Tests2;
public class TypeValidityTests
{
    [Fact]
    public void RefLevel0_TypesStillValid()
    {
        typeof(Stuff.Types.SourceAttribute).Should()
            .Implement<ISurrogateAttribute>();
    }

    [Fact]
    public void RefLevel1_TypesStillValid()
    {
        typeof(TestAssembly.RefLevel1.RefLevel1.SourceAttribute).Should()
            .Implement<ISurrogateAttribute>();
    }

    [Fact]
    public void RefLevel2_TypesStillValid()
    {
        typeof(TestAssembly.RefLevel2.Pattern.RefLevel2.SourceAttribute).Should()
            .Implement<ISurrogateAttribute>();
    }
}
