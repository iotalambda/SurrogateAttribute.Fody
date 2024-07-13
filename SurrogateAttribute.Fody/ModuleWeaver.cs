using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SurrogateAttribute.Fody
{
    public class ModuleWeaver : BaseModuleWeaver
    {
        public override void Execute()
        {
            try
            {
                ExecuteInternal();
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine($"### EXCEPTION: {e.Message}, \r\n{e.StackTrace}");
#endif
                throw;
            }
        }

        void ExecuteInternal()
        {
            var markerTypeDef = FindTypeDefinition("SurrogateAttribute.ISurrogateAttribute");
            var markerImpls = new List<MarkerImpl>();

            var usages = new List<Usage>();

            foreach (var typeDef in ModuleDefinition.GetTypes())
            {
                try
                {
                    {
                        var hasUsages = false;
                        hasUsages = SurrogateUsages.TryGetPropUsages(typeDef, markerTypeDef, in usages) || hasUsages;
                        hasUsages = SurrogateUsages.TryGetClassInterfUsages(typeDef, markerTypeDef, in usages) || hasUsages;
                        if (hasUsages)
                            continue;
                    }

                    if (!typeDef.HasInterfaces)
                        continue;

                    var markerImpl = typeDef.Interfaces.FirstOrDefault(i => i.InterfaceType.FullName == markerTypeDef.FullName);
                    if (markerImpl == null)
                        continue;

                    var tgtAttrsExprPropDef = typeDef.Properties.First(p => p.Name.Split(' ', '.').Last() == "TargetAttributes");
                    var instrs = tgtAttrsExprPropDef.GetMethod.Body.Instructions.ToArray();
                    var srcAttrValidTargets = GetValidAttrTargetsOrDefault(typeDef);

                    var currMarkerImpl = new MarkerImpl { TypeDef = typeDef };
                    markerImpls.Add(currMarkerImpl);
                    TgtAttr currTgtAttr = null;
                    MembBinding currMembBinding = null;

                    var exp = Exp.ExprArrA;
                    bool Expecting(Exp flags) => (exp & flags) != 0;
#if DEBUG
                    Console.WriteLine($"### INSTRS OF {typeDef.FullName}.{tgtAttrsExprPropDef.Name}:");
#endif
                    for (var ix = 0; ix < instrs.Length && exp != Exp.None; ix++)
                    {
                        var i = instrs[ix];
#if DEBUG
                        Console.WriteLine($"### {i}");
#endif
                        try
                        {
                            if (Expecting(Exp.ExprArrA))
                            {
                                if (i.OpCode.Code == Code.Newarr
                                    && i.Operand is TypeReference attributeRef
                                    && attributeRef.Name.EndsWith("Attribute"))
                                {
                                    ix += 2; // skip: dup, ldc.i4.0
                                    exp = Exp.TgtAttrNewObj
                                        | Exp.TgtCtorArgBinding_Src
                                        | Exp.ExprArrZ;
                                    continue;
                                }
                            }

                            if (Expecting(Exp.ExprArrZ))
                            {
                                if (i.OpCode.Code == Code.Ret)
                                {
                                    exp = Exp.None;
                                    continue;
                                }
                            }

                            if (Expecting(Exp.TgtAttrNewObj))
                            {
                                if (i.OpCode.Code == Code.Newobj
                                    && i.Operand is MethodReference ctorRef
                                    && ctorRef.Name == ".ctor")
                                {
                                    if (currTgtAttr == null)
                                    {
                                        currTgtAttr = new TgtAttr();
                                        currMarkerImpl.TgtAttrs.Add(currTgtAttr);
                                    }

                                    currTgtAttr.TypeRef = ctorRef.DeclaringType;
                                    currTgtAttr.CtorRef = ctorRef;

                                    if (srcAttrValidTargets != null)
                                    {
                                        var tgtAttrValidTargets = GetValidAttrTargetsOrDefault(currTgtAttr.TypeRef.Resolve());
                                        if ((srcAttrValidTargets.Value & tgtAttrValidTargets) != srcAttrValidTargets)
                                            throw new WeavingException($"'{currTgtAttr.TypeRef.Name}' is not compatible with the attribute targets of '{typeDef.Name}'.");
                                    }

                                    exp = Exp.TgtPropBinding_Src
                                        | Exp.TgtAttrZ;
                                    continue;
                                }
                            }

                            if (Expecting(Exp.TgtAttrZ))
                            {
                                if (i.OpCode.Code == Code.Stelem_Ref)
                                {
                                    currTgtAttr = null;
                                    ix += 2; // skip: dup, ldc.i4.0
                                    exp = Exp.TgtAttrNewObj
                                        | Exp.TgtCtorArgBinding_Src
                                        | Exp.ExprArrZ;
                                    continue;
                                }
                            }

                            if (Expecting(Exp.TgtPropBinding_Src))
                            {
                                // From Field mapped from Ctor Arg
                                if (i.OpCode.Code == Code.Ldfld
                                    && i.Operand is FieldReference srcFieldRef)
                                {
                                    currMembBinding = new MembBinding
                                    {
                                        SrcKind = SrcKind.FieldMappedFromCtorArg,
                                        SrcFieldRef = srcFieldRef,
                                        TgtKind = TgtKind.CtorArg,
                                    };
                                    currTgtAttr.MembBindings.Add(currMembBinding);
                                    exp = Exp.TgtPropBinding_Tgt;
                                    continue;
                                }

                                // From Property
                                if (i.OpCode.Code == Code.Call
                                    && i.Operand is MethodReference srcGetterRef
                                    && srcGetterRef.Name.StartsWith("get_"))
                                {
                                    var srcPropDef = srcGetterRef.DeclaringType.Resolve().Properties.First(p => p.GetMethod.FullName == srcGetterRef.FullName);
                                    var hasDefault = TryGetPropDefaultValue(srcPropDef, out var srcPropDefault);
                                    currMembBinding = new MembBinding
                                    {
                                        SrcKind = SrcKind.Property,
                                        SrcPropDef = srcPropDef,
                                        SrcPropHasDefault = hasDefault,
                                        SrcPropDefault = srcPropDefault,
                                        TgtKind = TgtKind.Property,
                                    };
                                    currTgtAttr.MembBindings.Add(currMembBinding);
                                    exp = Exp.TgtPropBinding_Tgt;
                                    continue;
                                }

                                // From Constant Number
                                if (IsLdcOpCode(i.OpCode))
                                {
                                    currMembBinding = new MembBinding
                                    {
                                        SrcKind = SrcKind.Constant,
                                        SrcConst = ValueFromLdcInstruction(i),
                                        TgtKind = TgtKind.Property
                                    };
                                    currTgtAttr.MembBindings.Add(currMembBinding);
                                    exp = Exp.TgtPropBinding_Tgt;
                                    continue;
                                }

                                // From Constant String
                                if (i.OpCode.Code == Code.Ldstr)
                                {
                                    currMembBinding = new MembBinding
                                    {
                                        SrcKind = SrcKind.Constant,
                                        SrcConst = (string)i.Operand,
                                        TgtKind = TgtKind.Property
                                    };
                                    currTgtAttr.MembBindings.Add(currMembBinding);
                                    exp = Exp.TgtPropBinding_Tgt;
                                    continue;
                                }

                                // From Type
                                if (i.OpCode.Code == Code.Ldtoken
                                    && i.Operand is TypeReference srcTypeRef)
                                {
                                    currMembBinding = new MembBinding
                                    {
                                        SrcKind = SrcKind.Constant,
                                        SrcConst = srcTypeRef,
                                        TgtKind = TgtKind.Property
                                    };
                                    currTgtAttr.MembBindings.Add(currMembBinding);
                                    exp = Exp.TgtPropBinding_Tgt;
                                    continue;
                                }
                            }

                            if (Expecting(Exp.TgtPropBinding_Tgt))
                            {
                                if (i.OpCode.Code == Code.Callvirt
                                    && i.Operand is MethodReference tgtSetterRef
                                    && tgtSetterRef.Name.StartsWith("set_"))
                                {
                                    var tgtPropDef = tgtSetterRef.DeclaringType.Resolve().Properties.First(p => p.SetMethod != null && p.SetMethod.FullName == tgtSetterRef.FullName);
                                    currMembBinding.TgtKind = TgtKind.Property;
                                    currMembBinding.TgtPropDef = tgtPropDef;
                                    currMembBinding = null;
                                    exp = Exp.TgtPropBinding_Src
                                        | Exp.TgtAttrZ;
                                    continue;
                                }
                            }

                            if (Expecting(Exp.TgtCtorArgBinding_Src))
                            {
                                // From Field mapped from Ctor Arg
                                if (i.OpCode.Code == Code.Ldfld
                                    && i.Operand is FieldReference srcFieldRef)
                                {
                                    if (currTgtAttr == null)
                                    {
                                        currTgtAttr = new TgtAttr();
                                        currMarkerImpl.TgtAttrs.Add(currTgtAttr);
                                    }

                                    currTgtAttr.MembBindings.Add(new MembBinding
                                    {
                                        SrcKind = SrcKind.FieldMappedFromCtorArg,
                                        SrcFieldRef = srcFieldRef,
                                        TgtKind = TgtKind.CtorArg,
                                    });
                                    exp = Exp.TgtCtorArgBinding_Src
                                        | Exp.TgtAttrNewObj;
                                    continue;
                                }

                                // From Property
                                if (i.OpCode.Code == Code.Call
                                    && i.Operand is MethodReference srcGetterRef
                                    && srcGetterRef.Name.StartsWith("get_"))
                                {
                                    if (currTgtAttr == null)
                                    {
                                        currTgtAttr = new TgtAttr();
                                        currMarkerImpl.TgtAttrs.Add(currTgtAttr);
                                    }

                                    var srcPropDef = srcGetterRef.DeclaringType.Resolve().Properties.First(p => p.GetMethod.FullName == srcGetterRef.FullName);
                                    var hasDefault = TryGetPropDefaultValue(srcPropDef, out var srcPropDefault);
                                    currTgtAttr.MembBindings.Add(new MembBinding
                                    {
                                        SrcKind = SrcKind.Property,
                                        SrcPropDef = srcPropDef,
                                        SrcPropHasDefault = hasDefault,
                                        SrcPropDefault = srcPropDefault,
                                        TgtKind = TgtKind.CtorArg,
                                    });
                                    exp = Exp.TgtCtorArgBinding_Src
                                        | Exp.TgtAttrNewObj;
                                    continue;
                                }

                                // From Constant Number
                                if (IsLdcOpCode(i.OpCode))
                                {
                                    if (currTgtAttr == null)
                                    {
                                        currTgtAttr = new TgtAttr();
                                        currMarkerImpl.TgtAttrs.Add(currTgtAttr);
                                    }

                                    currTgtAttr.MembBindings.Add(new MembBinding
                                    {
                                        SrcKind = SrcKind.Constant,
                                        SrcConst = ValueFromLdcInstruction(i),
                                        TgtKind = TgtKind.CtorArg,
                                    });
                                    exp = Exp.TgtCtorArgBinding_Src
                                        | Exp.TgtAttrNewObj;
                                    continue;
                                }

                                // From Constant String
                                if (i.OpCode.Code == Code.Ldstr)
                                {
                                    if (currTgtAttr == null)
                                    {
                                        currTgtAttr = new TgtAttr();
                                        currMarkerImpl.TgtAttrs.Add(currTgtAttr);
                                    }

                                    currTgtAttr.MembBindings.Add(new MembBinding
                                    {
                                        SrcKind = SrcKind.Constant,
                                        SrcConst = (string)i.Operand,
                                        TgtKind = TgtKind.CtorArg,
                                    });
                                    exp = Exp.TgtCtorArgBinding_Src
                                        | Exp.TgtAttrNewObj;
                                    continue;
                                }

                                // From Type
                                if (i.OpCode.Code == Code.Ldtoken
                                    && i.Operand is TypeReference srcTypeRef)
                                {
                                    if (currTgtAttr == null)
                                    {
                                        currTgtAttr = new TgtAttr();
                                        currMarkerImpl.TgtAttrs.Add(currTgtAttr);
                                    }

                                    currTgtAttr.MembBindings.Add(new MembBinding
                                    {
                                        SrcKind = SrcKind.Constant,
                                        SrcConst = srcTypeRef,
                                        TgtKind = TgtKind.CtorArg,
                                    });
                                    exp = Exp.TgtCtorArgBinding_Src
                                        | Exp.TgtAttrNewObj;
                                    continue;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            throw e.EnsureWeavingException(
                                $"An exception was thrown when processing 'TargetAttributes' of the surrogate attribute '{typeDef.Name}'.",
                                tgtAttrsExprPropDef.GetMethod?.DebugInformation.SequencePoints[0]);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e.EnsureWeavingException($"An exception was thrown when processing type '{typeDef.Name}'.");
                }
            }

            foreach (var usage in usages)
            {
                try
                {
                    var markerImpl = markerImpls.First(m => m.TypeDef == usage.AttrTypeDef);
                    usage.CustAttrProvider.CustomAttributes.Remove(usage.Attr);

                    foreach (var tgtAttr in markerImpl.TgtAttrs)
                    {
                        var newAttr = new CustomAttribute(tgtAttr.CtorRef);
                        int ctorParamIx = 0;
                        foreach (var membBinding in tgtAttr.MembBindings)
                        {
                            object value = null;
                            switch (membBinding.SrcKind)
                            {
                                case SrcKind.Property:
                                    var na = usage.Attr.Properties.FirstOrDefault(p => p.Name == membBinding.SrcPropDef.Name);
                                    if (na.Name != null)
                                        value = na.Argument.Value;
                                    else if (membBinding.SrcPropHasDefault)
                                        value = membBinding.SrcPropDefault;
                                    break;

                                case SrcKind.Constant:
                                    value = membBinding.SrcConst;
                                    break;

                                case SrcKind.FieldMappedFromCtorArg:
                                    if (TryGetAttrArgMappedToField(usage.Attr, membBinding.SrcFieldRef, out var ctorArgValue))
                                        value = ctorArgValue;
                                    break;

                                default:
                                    throw new KeyNotFoundException($"{nameof(membBinding.SrcKind)}:{membBinding.TgtKind}");
                            }

                            switch (membBinding.TgtKind)
                            {
                                case TgtKind.Property:
                                    if (!(value is TypeReference) && membBinding.TgtPropDef.PropertyType.FullName.StartsWith("System."))
                                        value = Convert.ChangeType(value, Type.GetType(membBinding.TgtPropDef.PropertyType.FullName));
                                    newAttr.Properties.Add(new CustomAttributeNamedArgument(membBinding.TgtPropDef.Name, new CustomAttributeArgument(membBinding.TgtPropDef.PropertyType, value)));
                                    break;
                                case TgtKind.CtorArg:
                                    var ctorParamDef = newAttr.Constructor.Parameters[ctorParamIx++];
                                    if (!(value is TypeReference) && ctorParamDef.ParameterType.FullName.StartsWith("System."))
                                        value = Convert.ChangeType(value, Type.GetType(ctorParamDef.ParameterType.FullName));
                                    newAttr.ConstructorArguments.Add(new CustomAttributeArgument(ctorParamDef.ParameterType, value));
                                    break;
                                default:
                                    throw new KeyNotFoundException($"{nameof(membBinding.SrcKind)}:{membBinding.TgtKind}");
                            }
                        }

                        usage.CustAttrProvider.CustomAttributes.Add(newAttr);
                    }
                }
                catch (Exception e)
                {
                    throw e.EnsureWeavingException($"An exception was thrown when processing a usage of the surrogate attribute '{usage.AttrTypeDef.Name}'.");
                }
            }

            foreach (var markerImpl in markerImpls)
                ModuleDefinition.Types.Remove(markerImpl.TypeDef);
        }

        public override IEnumerable<string> GetAssembliesForScanning()
        {
            yield return "netstandard";
            yield return "mscorlib";
            yield return "SurrogateAttribute";
        }

        public override bool ShouldCleanReference => true;

        static bool IsLdcOpCode(OpCode opCode)
        {
            return opCode.Code == Code.Ldc_I4_M1
                || opCode.Code == Code.Ldc_I4_0
                || opCode.Code == Code.Ldc_I4_1
                || opCode.Code == Code.Ldc_I4_2
                || opCode.Code == Code.Ldc_I4_3
                || opCode.Code == Code.Ldc_I4_4
                || opCode.Code == Code.Ldc_I4_5
                || opCode.Code == Code.Ldc_I4_6
                || opCode.Code == Code.Ldc_I4_7
                || opCode.Code == Code.Ldc_I4_8
                || opCode.Code == Code.Ldc_I4_S
                || opCode.Code == Code.Ldc_I4
                || opCode.Code == Code.Ldc_I8
                || opCode.Code == Code.Ldc_R4
                || opCode.Code == Code.Ldc_R8;
        }

        static object ValueFromLdcInstruction(Instruction i)
        {
            switch (i.OpCode.Code)
            {
                case Code.Ldc_I4_M1: return -1;
                case Code.Ldc_I4_0: return 0;
                case Code.Ldc_I4_1: return 1;
                case Code.Ldc_I4_2: return 2;
                case Code.Ldc_I4_3: return 3;
                case Code.Ldc_I4_4: return 4;
                case Code.Ldc_I4_5: return 5;
                case Code.Ldc_I4_6: return 6;
                case Code.Ldc_I4_7: return 7;
                case Code.Ldc_I4_8: return 8;
                case Code.Ldc_I4_S: return (sbyte)i.Operand;
                case Code.Ldc_I4: return (int)i.Operand;
                case Code.Ldc_I8: return (long)i.Operand;
                case Code.Ldc_R4: return (float)i.Operand;
                case Code.Ldc_R8: return (double)i.Operand;
                default: throw new NotSupportedException();
            }
        }

        static bool IsLdargOpCode(OpCode opCode)
        {
            return opCode.Code == Code.Ldarg
                || opCode.Code == Code.Ldarg_0
                || opCode.Code == Code.Ldarg_1
                || opCode.Code == Code.Ldarg_2
                || opCode.Code == Code.Ldarg_3
                || opCode.Code == Code.Ldarg_S;
        }

        static int ValueFromLdargInstruction(Instruction i)
        {
            switch (i.OpCode.Code)
            {
                case Code.Ldarg_0: return 0;
                case Code.Ldarg_1: return 1;
                case Code.Ldarg_2: return 2;
                case Code.Ldarg_3: return 3;
                case Code.Ldarg_S: return (sbyte)i.Operand;
                case Code.Ldarg: return (int)i.Operand;
                default: throw new NotSupportedException();
            }
        }

        static bool TryGetPropDefaultValue(PropertyDefinition propDef, out object value)
        {
            do
            {
                if (!propDef.HasCustomAttributes)
                    break;

                var propDefaultValueAttr = propDef.CustomAttributes.FirstOrDefault(a => a.AttributeType.FullName == "SurrogateAttribute.PropertyDefaultValueAttribute");
                if (propDefaultValueAttr == null)
                    break;

                var customAttrArgument = propDefaultValueAttr.ConstructorArguments[0];
                if (propDef.PropertyType != customAttrArgument.Type)
                    throw new WeavingException($"'PropertyDefaultValueAttribute({customAttrArgument.Type.Name})' does not match its property '{propDef.PropertyType.Name} {propDef.Name}'.") { SequencePoint = propDef.GetMethod?.DebugInformation.SequencePoints[0] };

                value = customAttrArgument.Value;
                return true;
            }
            while (false);

            value = null;
            return false;
        }

        static bool TryGetAttrArgMappedToField(CustomAttribute attr, FieldReference fieldRef, out object value)
        {
            do
            {
                var ctorDef = attr.Constructor.Resolve();

                if (!ctorDef.HasBody)
                    break;

                if (!ctorDef.HasParameters)
                    break;

                var instrs = ctorDef.Body.Instructions;
                foreach (var i in instrs)
                {
                    if (i.OpCode.Code == Code.Stfld
                        && i.Operand is FieldReference fieldRef2
                        && fieldRef == fieldRef2
                        && IsLdargOpCode(i.Previous.OpCode))
                    {
                        var attrCtorArgIx = ValueFromLdargInstruction(i.Previous) - 1;
                        var attrCtorArg = attr.ConstructorArguments.ElementAt(attrCtorArgIx);
                        value = attrCtorArg.Value;
                        return true;
                    }
                }

            }
            while (false);

            value = null;
            return false;
        }

        static AttributeTargets? GetValidAttrTargetsOrDefault(TypeDefinition attrTypeDef)
        {
            var targetsInt = (int?)(attrTypeDef.CustomAttributes
                .FirstOrDefault(a => a.AttributeType.FullName == "System.AttributeUsageAttribute")
                ?.ConstructorArguments.First().Value);

            if (targetsInt == null)
                return null;

            return (AttributeTargets)targetsInt;
        }
    }
}
