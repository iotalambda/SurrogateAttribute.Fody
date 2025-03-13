using Shouldly;
using TestAssembly;
using Tests.Stuff;
using Xunit.Abstractions;

namespace Tests;

public class Tests(ITestOutputHelper outputHelper, FodyTestResultInitializer<Types.Class> initializer) : TestsBase(outputHelper, initializer), IClassFixture<FodyTestResultInitializer<Types.Class>>
{
    [Fact]
    public void Types_Ok()
    {
        var propInfo = tr.GetTypeFromAssembly<Types.Class>().GetProperty(nameof(Types.Class.ClassProp));
        var propAttributes = propInfo.CustomAttributes.ToList();
        propAttributes.ShouldSatisfyRespectively([a =>
        {
            a.AttributeType.ShouldHaveSameFullNameAs<Types.TargetAttribute>();

            a.ConstructorArguments.ShouldSatisfyRespectively([
                t => t.ShouldHaveTypeAndValue(typeof(int[]), Types.Values.ArrayIntArg),
                t => t.ShouldHaveTypeAndValue(typeof(string[]), Types.Values.ArrayStringArg),
                t => t.ShouldHaveTypeAndValue(typeof(Type[]), Types.Values.ArrayTypeArg),
                t => t.ShouldHaveTypeAndValue(typeof(bool), Types.Values.BoolArg),
                t => t.ShouldHaveTypeAndValue(typeof(byte), Types.Values.ByteArg),
                t => t.ShouldHaveTypeAndValue(typeof(char), Types.Values.CharArg),
                t => t.ShouldHaveTypeAndValue(typeof(double), Types.Values.DoubleArg),
                t => t.ShouldHaveTypeAndValue(typeof(Types.MyEnum), (int)Types.Values.EnumArg),
                t => t.ShouldHaveTypeAndValue(typeof(float), Types.Values.FloatArg),
                t => t.ShouldHaveTypeAndValue(typeof(int), Types.Values.IntArg),
                t => t.ShouldHaveTypeAndValue(typeof(long), Types.Values.LongArg),
                t => t.ShouldHaveTypeAndValue(typeof(string), Types.Values.StringArg),
                t => t.ShouldHaveTypeAndValue(typeof(Type), Types.Values.TypeArg)]);

            a.NamedArguments.ShouldSatisfyRespectively([
                n => n.ShouldHaveNameTypeAndValue(nameof(Types.TargetAttribute.ArrayIntProp), typeof(int[]), Types.Values.ArrayIntProp),
                n => n.ShouldHaveNameTypeAndValue(nameof(Types.TargetAttribute.ArrayStringProp), typeof(string[]), Types.Values.ArrayStringProp),
                n => n.ShouldHaveNameTypeAndValue(nameof(Types.TargetAttribute.ArrayTypeProp), typeof(Type[]), Types.Values.ArrayTypeProp),
                n => n.ShouldHaveNameTypeAndValue(nameof(Types.TargetAttribute.BoolProp), typeof(bool), Types.Values.BoolProp),
                n => n.ShouldHaveNameTypeAndValue(nameof(Types.TargetAttribute.ByteProp), typeof(byte), Types.Values.ByteProp),
                n => n.ShouldHaveNameTypeAndValue(nameof(Types.TargetAttribute.CharProp), typeof(char), Types.Values.CharProp),
                n => n.ShouldHaveNameTypeAndValue(nameof(Types.TargetAttribute.DoubleProp), typeof(double), Types.Values.DoubleProp),
                n => n.ShouldHaveNameTypeAndValue(nameof(Types.TargetAttribute.EnumProp), typeof(Types.MyEnum), (int)Types.Values.EnumProp),
                n => n.ShouldHaveNameTypeAndValue(nameof(Types.TargetAttribute.FloatProp), typeof(float), Types.Values.FloatProp),
                n => n.ShouldHaveNameTypeAndValue(nameof(Types.TargetAttribute.IntProp), typeof(int), Types.Values.IntProp),
                n => n.ShouldHaveNameTypeAndValue(nameof(Types.TargetAttribute.LongProp), typeof(long), Types.Values.LongProp),
                n => n.ShouldHaveNameTypeAndValue(nameof(Types.TargetAttribute.StringProp), typeof(string), Types.Values.StringProp),
                n => n.ShouldHaveNameTypeAndValue(nameof(Types.TargetAttribute.TypeProp), typeof(Type), Types.Values.TypeProp)]);
        }]);
    }

    [Fact]
    public void ClassInterf_ClassOk()
    {
        var classType = tr.GetTypeFromAssembly<ClassInterf.Class>();
        var classAttributes = classType.CustomAttributes.ToList();
        classAttributes.ShouldSatisfyRespectively([a => a.AttributeType.ShouldHaveSameFullNameAs<ClassInterf.TargetClassAttribute>()]);
    }

    [Fact]
    public void ClassInterf_InterfOk()
    {
        var interfType = tr.GetTypeFromAssembly<ClassInterf.IInterf>();
        var interfAttributes = interfType.CustomAttributes.ToList();
        interfAttributes.ShouldSatisfyRespectively([a => a.AttributeType.ShouldHaveSameFullNameAs<ClassInterf.TargetInterfAttribute>()]);
    }

    [Fact]
    public void TargetAttrInheritance_Ok()
    {
        var classType = tr.GetTypeFromAssembly<TargetAttrInheritance.Class>();
        var classAttributes = classType.CustomAttributes.ToList();
        classAttributes.ShouldSatisfyRespectively([a =>
        {
            a.AttributeType.ShouldHaveSameFullNameAs<TargetAttrInheritance.TargetAttribute>();
            a.NamedArguments.ShouldSatisfyRespectively([
                n => n.ShouldHaveNameTypeAndValue(nameof(TargetAttrInheritance.TargetAttribute.ChildProp), typeof(string), TargetAttrInheritance.Values.ChildProp),
                n => n.ShouldHaveNameTypeAndValue(nameof(TargetAttrInheritance.TargetBaseAttribute.VirtualProp), typeof(string), TargetAttrInheritance.Values.VirtualProp),
                n => n.ShouldHaveNameTypeAndValue(nameof(TargetAttrInheritance.TargetBaseAttribute.BaseProp), typeof(string), TargetAttrInheritance.Values.BaseProp)]);
        }]);
    }

    [Fact]
    public void CtorMappingSources_Ok()
    {
        var propInfo = tr.GetTypeFromAssembly<CtorMappingSources.Class>().GetProperty(nameof(CtorMappingSources.Class.ClassProp));
        var propAttributes = propInfo.CustomAttributes.ToList();
        propAttributes.ShouldSatisfyRespectively([a =>
        {
            a.AttributeType.ShouldHaveSameFullNameAs<CtorMappingSources.TargetAttribute>();
            a.ConstructorArguments.ShouldSatisfyRespectively([
                t => t.ShouldHaveTypeAndValue(typeof(string), CtorMappingSources.Values.PropToArg),
                t => t.ShouldHaveTypeAndValue(typeof(string), CtorMappingSources.Values.ArgToArg)]);
            a.NamedArguments.ShouldSatisfyRespectively([
                n => n.ShouldHaveNameTypeAndValue(nameof(CtorMappingSources.TargetAttribute.ArgToProp), typeof(string), CtorMappingSources.Values.ArgToProp),
                n => n.ShouldHaveNameTypeAndValue(nameof(CtorMappingSources.TargetAttribute.StrConst), typeof(string), CtorMappingSources.Values.StrConst),
                n => n.ShouldHaveNameTypeAndValue(nameof(CtorMappingSources.TargetAttribute.StrLiteral), typeof(string), CtorMappingSources.Values.StrLiteral)
            ]);
        }]);
    }

    [Fact]
    public void CtorMappingTypes_Ok()
    {
        var propInfo = tr.GetTypeFromAssembly<CtorMappingTypes.Class>().GetProperty(nameof(CtorMappingTypes.Class.ClassProp));
        var propAttributes = propInfo.CustomAttributes.ToList();
        propAttributes.ShouldSatisfyRespectively([a =>
        {
            a.AttributeType.ShouldHaveSameFullNameAs<CtorMappingTypes.TargetAttribute>();
            a.ConstructorArguments.ShouldSatisfyRespectively([
                t => t.ShouldHaveTypeAndValue(typeof(bool), CtorMappingTypes.Values.BoolArg),
                t => t.ShouldHaveTypeAndValue(typeof(byte), CtorMappingTypes.Values.ByteArg),
                t => t.ShouldHaveTypeAndValue(typeof(char), CtorMappingTypes.Values.CharArg),
                t => t.ShouldHaveTypeAndValue(typeof(double), CtorMappingTypes.Values.DoubleArg),
                t => t.ShouldHaveTypeAndValue(typeof(CtorMappingTypes.MyEnum), (int)CtorMappingTypes.Values.EnumArg),
                t => t.ShouldHaveTypeAndValue(typeof(float), CtorMappingTypes.Values.FloatArg),
                t => t.ShouldHaveTypeAndValue(typeof(int), CtorMappingTypes.Values.IntArg),
                t => t.ShouldHaveTypeAndValue(typeof(long), CtorMappingTypes.Values.LongArg),
                t => t.ShouldHaveTypeAndValue(typeof(string), CtorMappingTypes.Values.StringArg),
                t => t.ShouldHaveTypeAndValue(typeof(Type), CtorMappingTypes.Values.TypeArg)
            ]);
        }]);
    }

    [Fact]
    public void PropSources_Ok()
    {
        var classType = tr.GetTypeFromAssembly<PropSources.Class>();
        var classAttributes = classType.CustomAttributes.ToList();
        classAttributes.ShouldSatisfyRespectively([a =>
        {
            a.AttributeType.ShouldHaveSameFullNameAs<PropSources.TargetAttribute>();
            a.NamedArguments.ShouldSatisfyRespectively([
                n => n.ShouldHaveNameTypeAndValue(nameof(PropSources.TargetAttribute.StringPropFromCtorArg), typeof(string), PropSources.Values.StringPropFromCtorArg),
                n => n.ShouldHaveNameTypeAndValue(nameof(PropSources.TargetAttribute.StringPropFromNamedArg), typeof(string), PropSources.Values.StringPropFromNamedArg),
                n => n.ShouldHaveNameTypeAndValue(nameof(PropSources.TargetAttribute.StringPropFromDefault), typeof(string), PropSources.Values.StringPropFromDefault),
                n => n.ShouldHaveNameTypeAndValue(nameof(PropSources.TargetAttribute.StringPropFromConst), typeof(string), PropSources.Values.StringPropFromConst),
                n => n.ShouldHaveNameTypeAndValue(nameof(PropSources.TargetAttribute.ArrayStringPropFromCtorArg), typeof(string[]), PropSources.Values.ArrayStringPropFromCtorArg),
                n => n.ShouldHaveNameTypeAndValue(nameof(PropSources.TargetAttribute.ArrayStringPropFromNamedArg), typeof(string[]), PropSources.Values.ArrayStringPropFromNamedArg),
                n => n.ShouldHaveNameTypeAndValue(nameof(PropSources.TargetAttribute.ArrayStringPropFromDefault), typeof(string[]), PropSources.Values.ArrayStringPropFromDefault),
                n => n.ShouldHaveNameTypeAndValue(nameof(PropSources.TargetAttribute.TypePropFromCtorArg), typeof(Type), PropSources.Values.TypePropFromCtorArg),
                n => n.ShouldHaveNameTypeAndValue(nameof(PropSources.TargetAttribute.TypePropFromNamedArg), typeof(Type), PropSources.Values.TypePropFromNamedArg),
                n => n.ShouldHaveNameTypeAndValue(nameof(PropSources.TargetAttribute.TypePropFromDefault), typeof(Type), PropSources.Values.TypePropFromDefault),
                n => n.ShouldHaveNameTypeAndValue(nameof(PropSources.TargetAttribute.TypePropFromConst), typeof(Type), PropSources.Values.TypePropFromConst)
            ]);
        }]);
    }

    [Fact]
    public void Multiple_Ok()
    {
        var classType = tr.GetTypeFromAssembly<Multiple.Class>();
        var classAttributes = classType.CustomAttributes.ToList();
        classAttributes.ShouldSatisfyRespectively([
            a11 =>
            {
                a11.AttributeType.ShouldHaveSameFullNameAs<Multiple.Source1Target1Attribute>();
                a11.NamedArguments.ShouldSatisfyRespectively([n => n.ShouldHaveNameTypeAndValue(nameof(Multiple.Source1Target1Attribute.Source1Target1Prop), typeof(string), Multiple.Values.Source1Target1Prop)]);
            },
            a12 =>
            {
                a12.AttributeType.ShouldHaveSameFullNameAs<Multiple.Source1Target2Attribute>();
                a12.NamedArguments.ShouldSatisfyRespectively([
                    n => n.ShouldHaveNameTypeAndValue(nameof(Multiple.Source1Target2Attribute.Source1Target2Prop1), typeof(string), Multiple.Values.Source1Target2Prop1),
                    n => n.ShouldHaveNameTypeAndValue(nameof(Multiple.Source1Target2Attribute.Source1Target2Prop2), typeof(string), Multiple.Values.Source1Target2Prop2)
                ]);
            },
            a13 =>
            {
                a13.AttributeType.ShouldHaveSameFullNameAs<Multiple.Source1Target3Attribute>();
                a13.NamedArguments.ShouldSatisfyRespectively([n => n.ShouldHaveNameTypeAndValue(nameof(Multiple.Source1Target3Attribute.Source1Target3Prop), typeof(string), Multiple.Values.Source1Target3Prop)]);
            },
            a21 =>
            {
                a21.AttributeType.ShouldHaveSameFullNameAs<Multiple.Source2Target1Attribute>();
                a21.NamedArguments.ShouldSatisfyRespectively([n => n.ShouldHaveNameTypeAndValue(nameof(Multiple.Source2Target1Attribute.Source2Target1Prop), typeof(string), Multiple.Values.Source2Target1Prop)]);
            },
            a22 =>
            {
                a22.AttributeType.ShouldHaveSameFullNameAs<Multiple.Source2Target2Attribute>();
                a22.NamedArguments.ShouldSatisfyRespectively([n => n.ShouldHaveNameTypeAndValue(nameof(Multiple.Source2Target2Attribute.Source2Target2Prop), typeof(string), Multiple.Values.Source2Target2Prop)]);
            }
        ]);
    }

    [Fact]
    public void Covariance_Ok()
    {
        var classType = tr.GetTypeFromAssembly<Covariance.Class>();
        var classAttributes = classType.CustomAttributes.ToList();
        classAttributes.ShouldSatisfyRespectively([a =>
        {
            a.AttributeType.ShouldHaveSameFullNameAs<Covariance.TargetAttribute>();
            a.ConstructorArguments.ShouldSatisfyRespectively([t => t.ShouldHaveTypeAndValue(typeof(object[]), Covariance.Values.ArrayStringPropFromNamedArg)]);
        }]);
    }
}