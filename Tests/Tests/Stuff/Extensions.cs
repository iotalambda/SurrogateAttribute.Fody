namespace Tests.Stuff;

internal static class Extensions
{
    public static Type GetTypeFromAssembly<TType>(this Fody.TestResult tr) => tr.Assembly.GetType(typeof(TType).FullName);
}
