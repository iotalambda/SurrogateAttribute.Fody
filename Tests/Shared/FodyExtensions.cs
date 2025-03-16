namespace Shared;

public static class FodyExtensions
{
    public static Type GetTypeFromAssembly<TType>(this Fody.TestResult tr) => tr.Assembly.GetType(typeof(TType).FullName);
}
