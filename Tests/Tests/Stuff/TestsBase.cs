using Fody;
using Xunit.Abstractions;

namespace Tests.Stuff;

public abstract class TestsBase : IDisposable
{
    readonly TestOutputRedirection redir;
    protected readonly TestResult tr;

    protected TestsBase(ITestOutputHelper outputHelper, IFodyTestResultInitializer initializer = null)
    {
        redir = new(outputHelper);
        if (initializer != null)
            tr = initializer.Initialize();
    }

    public void Dispose() => redir.Dispose();
}