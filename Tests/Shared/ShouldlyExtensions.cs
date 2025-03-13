using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Shouldly;

[DebuggerStepThrough]
[ShouldlyMethods]
[EditorBrowsable(EditorBrowsableState.Never)]
public static partial class ShouldlyExtensions
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ShouldImplement<T>(this Type actual, string customMessage = null)
    {
        if (!actual.IsAssignableTo(typeof(T)))
            throw new ShouldAssertException(new ExpectedShouldlyMessage(typeof(T).Name, customMessage).ToString());
    }


    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ShouldSatisfyRespectively<T>(this IEnumerable<T> actual, IEnumerable<Action<T>> assertions, string customMessage = null)
    {
        var actualArray = actual.ToArray();
        var assertionsArray = assertions.ToArray();

        if (actualArray.Length != assertionsArray.Length)
            throw new ShouldAssertException(new ExpectedShouldlyMessage(nameof(assertions), customMessage).ToString());

        foreach (var (actualItem, assertionItem) in actualArray.Zip(assertionsArray))
        {
            assertionItem(actualItem);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ShouldHaveSameFullNameAs(this Type actual, Type expected, string customMessage = null)
        => actual.FullName.ShouldBe(expected.FullName, customMessage);

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ShouldHaveSameFullNameAs<TExpected>(this Type actual, string customMessage = null)
        => actual.ShouldHaveSameFullNameAs(typeof(TExpected), customMessage);

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ShouldHaveTypeAndValue(this CustomAttributeTypedArgument actual, Type expectedPropType, object expectedPropValue, string customMessage = null)
    {
        actual.ArgumentType.ShouldHaveSameFullNameAs(expectedPropType);

        if (expectedPropType == typeof(Type))
        {
            if (actual.Value is not Type tValueType)
                throw new ShouldAssertException(new ExpectedShouldlyMessage($"{expectedPropType.Name}, {expectedPropValue}", customMessage).ToString());
            if (expectedPropValue is not Type propValueType)
                throw new ShouldAssertException(new ExpectedShouldlyMessage($"{expectedPropType.Name}, {expectedPropValue}", customMessage).ToString());

            tValueType.ShouldHaveSameFullNameAs(propValueType, customMessage);
        }
        else if (expectedPropType.IsAssignableTo(typeof(IList)) && actual.Value is IList<CustomAttributeTypedArgument> tList && expectedPropValue is IList propList)
        {
            if (tList.Count != propList.Count)
                throw new ShouldAssertException(new ExpectedShouldlyMessage($"{expectedPropType.Name}, {expectedPropValue}", customMessage).ToString());

            foreach (var (tListItem, propListItem) in tList.Zip(propList.Cast<object>()))
            {
                tListItem.Value.ShouldBe(propListItem);
            }
        }
        else
        {
            actual.Value.ShouldBe(expectedPropValue);
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ShouldHaveNameTypeAndValue(this CustomAttributeNamedArgument actual, string expectedPropName, Type expectedPropType, object expectedPropValue, string customMessage = null)
    {
        actual.MemberName.ShouldBe(expectedPropName, customMessage);
        actual.TypedValue.ShouldHaveTypeAndValue(expectedPropType, expectedPropValue, customMessage);
    }
}
