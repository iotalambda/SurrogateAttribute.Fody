using Shouldly;
using SurrogateAttribute;

namespace Tests2;
public class TypeValidityTests
{
    [Fact]
    public void RefLevel0_TypesStillValid()
    {
        var type = typeof(Stuff.Types.SourceAttribute);
        type.ShouldImplement<ISurrogateAttribute>();
    }

    [Fact]
    public void RefLevel1_TypesStillValid()
    {
        var type = typeof(TestAssembly.RefLevel1.RefLevel1.SourceAttribute);
        type.ShouldImplement<ISurrogateAttribute>();
    }

    [Fact]
    public void RefLevel2_TypesStillValid()
    {
        var type = typeof(TestAssembly.RefLevel2.Pattern.RefLevel2.SourceAttribute);
        type.ShouldImplement<ISurrogateAttribute>();
    }
}
