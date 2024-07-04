using Xunit.Abstractions;

namespace Tests;

public abstract class TestsBase(ITestOutputHelper outputHelper) : IDisposable
{
    private readonly TestOutputRedirection redir = new(outputHelper);

    public void Dispose() => redir.Dispose();
}
