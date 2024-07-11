using Fody;
using SurrogateAttribute.Fody;

namespace Tests.Stuff;

public class FodyTestResultInitializer<TMarker>
{
    public TestResult TestResult { get; } = new ModuleWeaver().ExecuteTestRun($"{typeof(TMarker).Assembly.GetName().Name}.dll");
}
