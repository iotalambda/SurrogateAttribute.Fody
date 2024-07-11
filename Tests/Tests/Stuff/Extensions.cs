using FluentAssertions;
using FluentAssertions.Types;
using System.Reflection;

namespace Tests.Stuff;

internal static class Extensions
{
    public static AndConstraint<TypeAssertions> HaveSameFullNameAs(this TypeAssertions assertions, Type other, string because = "", params object[] becauseArgs)
        => assertions.Match(t => t.FullName == other.FullName, because, becauseArgs);

    public static AndConstraint<TypeAssertions> HaveSameFullNameAs<TType>(this TypeAssertions assertions, string because = "", params object[] becauseArgs)
        => assertions.HaveSameFullNameAs(typeof(TType), because, becauseArgs);

    public static void ShouldHaveTypeAndValue(this CustomAttributeTypedArgument t, Type propType, object propValue)
    {
        t.ArgumentType.Should().HaveSameFullNameAs(propType);
        if (propType == typeof(Type)) t.Value.As<Type>().Should().HaveSameFullNameAs(propValue.As<Type>());
        else t.Value.Should().Be(propValue);
    }

    public static void ShouldHaveNameTypeAndValue(this CustomAttributeNamedArgument n, string propName, Type propType, object propValue)
    {
        n.MemberName.Should().Be(propName);
        n.TypedValue.ShouldHaveTypeAndValue(propType, propValue);
    }

    public static Type GetTypeFromAssembly<TType>(this Fody.TestResult tr) => tr.Assembly.GetType(typeof(TType).FullName);
}
