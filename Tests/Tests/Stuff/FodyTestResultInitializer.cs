using Fody;
using SurrogateAttribute.Fody;

namespace Tests.Stuff;

public class FodyTestResultInitializer<TMarker> : IFodyTestResultInitializer
{
    public TestResult Initialize() => new ModuleWeaver().ExecuteTestRun($"{typeof(TMarker).Assembly.GetName().Name}.dll");
}


public interface IFodyTestResultInitializer
{
    TestResult Initialize();
}