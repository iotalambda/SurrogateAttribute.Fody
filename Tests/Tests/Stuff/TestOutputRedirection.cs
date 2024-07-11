using System.Text;
using Xunit.Abstractions;

namespace Tests.Stuff;

internal class TestOutputRedirection : IDisposable
{
    class TestOutputWriter(ITestOutputHelper outputHelper) : TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;

        StringBuilder sb = new();

        public override void Write(char value)
        {
            if (value == '\n')
            {
                outputHelper.WriteLine(sb.ToString());
                sb.Clear();
            }
            else
            {
                sb.Append(value);
            }
        }

        public override void Flush()
        {
            if (sb.Length == 0) return;
            outputHelper.WriteLine(sb.ToString());
            sb.Clear();
        }
    }

    readonly TextWriter original;
    readonly TestOutputWriter testOutputWriter;

    public TestOutputRedirection(ITestOutputHelper outputHelper)
    {
        testOutputWriter = new TestOutputWriter(outputHelper);
        original = Console.Out;
        Console.SetOut(testOutputWriter);
    }

    public void Dispose()
    {
        Console.SetOut(original);
        testOutputWriter.Dispose();
    }
}
