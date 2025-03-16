
using Shared;
using Shouldly;
using TestAssembly.Readme;
using Xunit.Abstractions;

namespace Tests.Readme;

public class ExampleTests(ITestOutputHelper outputHelper, FodyTestResultInitializer<Example1.Class> initializer) : TestsBase(outputHelper, initializer), IClassFixture<FodyTestResultInitializer<Example1.Class>>
{
    [Fact]
    public void Example1_Ok()
    {
        var classType = typeof(Example1.Class);
        var classAttributes = classType.CustomAttributes.ToList();
        classAttributes.ShouldSatisfyRespectively([a =>
        {
            a.AttributeType.ShouldHaveSameFullNameAs<Example1.TargetAttribute>();
            a.ConstructorArguments.ShouldSatisfyRespectively([
                t => t.ShouldHaveTypeAndValue(typeof(bool), Example1.Values.CtorArg1),
                t => t.ShouldHaveTypeAndValue(typeof(byte), Example1.Values.CtorArg2),
                t => t.ShouldHaveTypeAndValue(typeof(char), Example1.Values.CtorArg3),
                t => t.ShouldHaveTypeAndValue(typeof(long), Example1.Values.CtorArg8),
                t => t.ShouldHaveTypeAndValue(typeof(sbyte), Example1.Values.CtorArg9),
                t => t.ShouldHaveTypeAndValue(typeof(short), Example1.Values.CtorArg10),
                t => t.ShouldHaveTypeAndValue(typeof(string), Example1.Values.CtorArg11),
                t => t.ShouldHaveTypeAndValue(typeof(bool[]), Example1.Values.CtorArg16),
                t => t.ShouldHaveTypeAndValue(typeof(byte[]), Example1.Values.CtorArg17),
                t => t.ShouldHaveTypeAndValue(typeof(char[]), Example1.Values.CtorArg18),
                t => t.ShouldHaveTypeAndValue(typeof(object[]), Example1.Values.CtorArg24),
                t => t.ShouldHaveTypeAndValue(typeof(sbyte[]), Example1.Values.CtorArg25),
                t => t.ShouldHaveTypeAndValue(typeof(short[]), Example1.Values.CtorArg26),
                t => t.ShouldHaveTypeAndValue(typeof(string[]), Example1.Values.CtorArg27),
                t => t.ShouldHaveTypeAndValue(typeof(bool), Example1.Values.NamedArg1),
                t => t.ShouldHaveTypeAndValue(typeof(byte), Example1.Values.NamedArg2),
                t => t.ShouldHaveTypeAndValue(typeof(char), Example1.Values.NamedArg3),
                t => t.ShouldHaveTypeAndValue(typeof(double), Example1.Values.NamedArg5),
                t => t.ShouldHaveTypeAndValue(typeof(float), Example1.Values.NamedArg6),
                t => t.ShouldHaveTypeAndValue(typeof(int), Example1.Values.NamedArg7),
                t => t.ShouldHaveTypeAndValue(typeof(bool[]), Example1.Values.NamedArg16),
                t => t.ShouldHaveTypeAndValue(typeof(byte[]), Example1.Values.NamedArg17),
                t => t.ShouldHaveTypeAndValue(typeof(char[]), Example1.Values.NamedArg18),
                t => t.ShouldHaveTypeAndValue(typeof(double[]), Example1.Values.NamedArg20),
                t => t.ShouldHaveTypeAndValue(typeof(float[]), Example1.Values.NamedArg21),
                t => t.ShouldHaveTypeAndValue(typeof(int[]), Example1.Values.NamedArg22),
                t => t.ShouldHaveTypeAndValue(typeof(long[]), Example1.Values.NamedArg23),
                t => t.ShouldHaveTypeAndValue(typeof(bool), Example1.Values.Constant1),
                t => t.ShouldHaveTypeAndValue(typeof(byte), Example1.Values.Constant2),
                t => t.ShouldHaveTypeAndValue(typeof(char), Example1.Values.Constant3),
                t => t.ShouldHaveTypeAndValue(typeof(double), Example1.Values.Constant5),
                t => t.ShouldHaveTypeAndValue(typeof(float), Example1.Values.Constant6),
                t => t.ShouldHaveTypeAndValue(typeof(int), Example1.Values.Constant7),
                t => t.ShouldHaveTypeAndValue(typeof(bool), Example1.Values.InlineCtorArg1),
                t => t.ShouldHaveTypeAndValue(typeof(byte), Example1.Values.InlineCtorArg2),
                t => t.ShouldHaveTypeAndValue(typeof(char), Example1.Values.InlineCtorArg3),
                t => t.ShouldHaveTypeAndValue(typeof(double), Example1.Values.InlineCtorArg5),
                t => t.ShouldHaveTypeAndValue(typeof(float), Example1.Values.InlineCtorArg6),
                t => t.ShouldHaveTypeAndValue(typeof(int), Example1.Values.InlineCtorArg7),
                t => t.ShouldHaveTypeAndValue(typeof(long), Example1.Values.InlineCtorArg8),
                t => t.ShouldHaveTypeAndValue(typeof(bool[]), Example1.Values.InlineCtorArg16),
                t => t.ShouldHaveTypeAndValue(typeof(byte[]), Example1.Values.InlineCtorArg17),
                t => t.ShouldHaveTypeAndValue(typeof(char[]), Example1.Values.InlineCtorArg18),
                t => t.ShouldHaveTypeAndValue(typeof(double[]), Example1.Values.InlineCtorArg20),
                t => t.ShouldHaveTypeAndValue(typeof(float[]), Example1.Values.InlineCtorArg21),
                t => t.ShouldHaveTypeAndValue(typeof(int[]), Example1.Values.InlineCtorArg22),
                t => t.ShouldHaveTypeAndValue(typeof(long[]), Example1.Values.InlineCtorArg23)
            ]);

            a.NamedArguments.ShouldSatisfyRespectively([
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.CtorArg5), typeof(double), Example1.Values.CtorArg5),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.CtorArg6), typeof(float), Example1.Values.CtorArg6),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.CtorArg7), typeof(int), Example1.Values.CtorArg7),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.CtorArg12), typeof(Type), Example1.Values.CtorArg12),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.CtorArg13), typeof(uint), Example1.Values.CtorArg13),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.CtorArg14), typeof(ulong), Example1.Values.CtorArg14),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.CtorArg15), typeof(ushort), Example1.Values.CtorArg15),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.CtorArg20), typeof(double[]), Example1.Values.CtorArg20),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.CtorArg21), typeof(float[]), Example1.Values.CtorArg21),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.CtorArg22), typeof(int[]), Example1.Values.CtorArg22),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.CtorArg23), typeof(long[]), Example1.Values.CtorArg23),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.CtorArg29), typeof(uint[]), Example1.Values.CtorArg29),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.CtorArg30), typeof(ulong[]), Example1.Values.CtorArg30),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.CtorArg31), typeof(ushort[]), Example1.Values.CtorArg31),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.NamedArg8), typeof(long), Example1.Values.NamedArg8),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.NamedArg9), typeof(sbyte), Example1.Values.NamedArg9),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.NamedArg10), typeof(short), Example1.Values.NamedArg10),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.NamedArg11), typeof(string), Example1.Values.NamedArg11DefaultValue),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.NamedArg12), typeof(Type), Example1.Values.NamedArg12),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.NamedArg13), typeof(uint), Example1.Values.NamedArg13),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.NamedArg14), typeof(ulong), Example1.Values.NamedArg14),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.NamedArg15), typeof(ushort), Example1.Values.NamedArg15),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.NamedArg24), typeof(object[]), Example1.Values.NamedArg24),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.NamedArg25), typeof(sbyte[]), Example1.Values.NamedArg25),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.NamedArg26), typeof(short[]), Example1.Values.NamedArg26),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.NamedArg27), typeof(string[]), Example1.Values.NamedArg27),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.NamedArg29), typeof(uint[]), Example1.Values.NamedArg29),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.NamedArg30), typeof(ulong[]), Example1.Values.NamedArg30),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.NamedArg31), typeof(ushort[]), Example1.Values.NamedArg31),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.Constant8), typeof(long), Example1.Values.Constant8),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.Constant9), typeof(sbyte), Example1.Values.Constant9),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.Constant10), typeof(short), Example1.Values.Constant10),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.Constant11), typeof(string), Example1.Values.Constant11),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.Constant12), typeof(uint), Example1.Values.Constant12),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.Constant13), typeof(ulong), Example1.Values.Constant13),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.Constant14), typeof(ushort), Example1.Values.Constant14),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.InlineNamedArg9), typeof(sbyte), Example1.Values.InlineNamedArg9),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.InlineNamedArg10), typeof(short), Example1.Values.InlineNamedArg10),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.InlineNamedArg11), typeof(string), Example1.Values.InlineNamedArg11),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.InlineNamedArg12), typeof(Type), Example1.Values.InlineNamedArg12),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.InlineNamedArg13), typeof(uint), Example1.Values.InlineNamedArg13),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.InlineNamedArg14), typeof(ulong), Example1.Values.InlineNamedArg14),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.InlineNamedArg15), typeof(ushort), Example1.Values.InlineNamedArg15),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.InlineNamedArg25), typeof(sbyte[]), Example1.Values.InlineNamedArg25),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.InlineNamedArg26), typeof(short[]), Example1.Values.InlineNamedArg26),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.InlineNamedArg27), typeof(string[]), Example1.Values.InlineNamedArg27),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.InlineNamedArg29), typeof(uint[]), Example1.Values.InlineNamedArg29),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.InlineNamedArg30), typeof(ulong[]), Example1.Values.InlineNamedArg30),
                n => n.ShouldHaveNameTypeAndValue(nameof(Example1.TargetAttribute.InlineNamedArg31), typeof(ushort[]), Example1.Values.InlineNamedArg31)
            ]);
        }]);
    }
}