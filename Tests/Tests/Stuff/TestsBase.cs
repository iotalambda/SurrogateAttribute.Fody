using Xunit.Abstractions;

namespace Tests.Stuff;

public abstract class TestsBase(ITestOutputHelper outputHelper) : IDisposable
{
    readonly TestOutputRedirection redir = new(outputHelper);

    public void Dispose() => redir.Dispose();
}
