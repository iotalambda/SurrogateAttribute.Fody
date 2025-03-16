using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SurrogateAttribute.Fody
{
    public class ModuleWeaver : BaseModuleWeaver
    {
        public override void Execute()
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

                    var markerInterf = typeDef.Interfaces.FirstOrDefault(i => i.InterfaceType.FullName == markerTypeDef.FullName);
                    if (markerInterf == null)
                        continue;

                    var markerImpl = TypeDefToMarkerImpl(typeDef);

                    markerImpls.Add(markerImpl);
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
                    var markerImpl = markerImpls.FirstOrDefault(m => m.TypeDef == usage.AttrTypeDef);
                    if (markerImpl == null)
                    {
                        markerImpl = TypeDefToMarkerImpl(usage.AttrTypeDef);
                        if (markerImpl == null)
                            throw new WeavingException($"Could not resolve the implementation of '{usage.AttrTypeDef.Name}'.");
                        markerImpls.Add(markerImpl);
                    }

                    usage.CustAttrProvider.CustomAttributes.Remove(usage.Attr);

                    foreach (var tgtAttr in markerImpl.TgtAttrs)
                    {
                        var newAttr = new CustomAttribute(usage.ModuleDef.ImportReference(tgtAttr.CtorRef));
                        int ctorParamIx = 0;
                        foreach (var membBinding in tgtAttr.MembBindings)
                        {
                            object value = null;
                            TypeReference typeRef = null;
                            switch (membBinding.SrcKind)
                            {
                                case SrcKind.Property:
                                    var na = usage.Attr.Properties.FirstOrDefault(p => p.Name == membBinding.SrcPropDef.Name);
                                    if (na.Name != null)
                                    {
                                        value = na.Argument.Value;
                                        typeRef = na.Argument.Type;
                                    }
                                    else if (membBinding.SrcPropHasDefault)
                                    {
                                        value = membBinding.SrcPropDefaultValue;
                                        typeRef = membBinding.SrcPropDefaultTypeRef;
                                    }
                                    break;

                                case SrcKind.Constant:
                                    value = membBinding.SrcConst;
                                    typeRef = usage.ModuleDef.ImportReference(value.GetType());
                                    break;

                                case SrcKind.FieldMappedInCtor:
                                    if (TryGetValueMappedToFieldInCtor(usage.Attr, membBinding.SrcFieldRef, usage, out var ctorMappedValue, out var ctorMappedTypeRef))
                                    {
                                        value = ctorMappedValue;
                                        typeRef = ctorMappedTypeRef;
                                    }
                                    break;

                                default:
                                    throw new KeyNotFoundException($"{nameof(membBinding.SrcKind)}:{membBinding.TgtKind}");
                            }

                            if (value is TypeReference typeRefValue)
                                value = usage.ModuleDef.ImportReference(typeRefValue);
                            else if (value is CustomAttributeArgument[] customAttrArguments)
                            {
                                var arr = Array.CreateInstance(Type.GetType(typeRef.FullName).GetElementType(), customAttrArguments.Length);
                                for (var i = 0; i < customAttrArguments.Length; i++)
                                    arr.SetValue(customAttrArguments[i].Value, i);
                                value = arr;
                            }

                            switch (membBinding.TgtKind)
                            {
                                case TgtKind.Property:
                                    if (value is TypeReference) { }
                                    else if (value is IList arr)
                                    {
                                        var srcElType = typeRef.GetElementType();
                                        var tgtElType = membBinding.TgtPropDef.PropertyType.GetElementType();
                                        var arrValue = Array.CreateInstance(typeof(CustomAttributeArgument), arr.Count);
                                        for (var i = 0; i < arr.Count; i++)
                                        {
                                            var elValue = new CustomAttributeArgument(srcElType, arr[i]);
                                            if (srcElType.FullName != tgtElType.FullName)
                                                if (tgtElType.FullName != typeof(Type).FullName)
                                                    elValue = new CustomAttributeArgument(tgtElType, elValue);
                                            arrValue.SetValue(elValue, i);
                                        }
                                        value = arrValue;
                                    }
                                    else if (membBinding.TgtPropDef.PropertyType.FullName.StartsWith("System."))
                                    {
                                        value = Convert.ChangeType(value, Type.GetType(membBinding.TgtPropDef.PropertyType.FullName));
                                    }
                                    newAttr.Properties.Add(new CustomAttributeNamedArgument(membBinding.TgtPropDef.Name, new CustomAttributeArgument(membBinding.TgtPropDef.PropertyType, value)));
                                    break;

                                case TgtKind.CtorArg:
                                    var ctorParamDef = newAttr.Constructor.Parameters[ctorParamIx++];
                                    if (value is TypeReference) { }
                                    else if (value is IList arr)
                                    {
                                        var srcElType = typeRef.GetElementType();
                                        var tgtElType = ctorParamDef.ParameterType.GetElementType();
                                        var arrValue = Array.CreateInstance(typeof(CustomAttributeArgument), arr.Count);
                                        for (var i = 0; i < arr.Count; i++)
                                        {
                                            var elValue = new CustomAttributeArgument(srcElType, arr[i]);
                                            if (srcElType.FullName != tgtElType.FullName)
                                                if (tgtElType.FullName != typeof(Type).FullName)
                                                    elValue = new CustomAttributeArgument(tgtElType, elValue);
                                            arrValue.SetValue(elValue, i);
                                        }
                                        value = arrValue;
                                    }
                                    else if (ctorParamDef.ParameterType.FullName.StartsWith("System."))
                                    {
                                        value = Convert.ChangeType(value, Type.GetType(ctorParamDef.ParameterType.FullName));
                                    }

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
        }

        static MarkerImpl TypeDefToMarkerImpl(TypeDefinition typeDef)
        {
            var tgtAttrsExprPropDef = typeDef.Properties.First(p => p.Name.Split(' ', '.').Last() == "TargetAttributes");
            var instrs = tgtAttrsExprPropDef.GetMethod.Body.Instructions.ToArray();
            var srcAttrValidTargets = GetValidAttrTargetsOrDefault(typeDef);

            var currMarkerImpl = new MarkerImpl { TypeDef = typeDef };
            TgtAttr currTgtAttr = null;
            void NewCurrTgtAttrIfNull()
            {
                if (currTgtAttr != null)
                    return;
                currTgtAttr = new TgtAttr();
                currMarkerImpl.TgtAttrs.Add(currTgtAttr);
            }
            MembBinding currMembBinding = null;

            var skip = 0;
            var exp = Exp.ExprArrA;
            bool Expecting(Exp flags) => (exp & flags) != 0;

            WriteLineIfDebug($"### INSTRS OF {typeDef.FullName}.{tgtAttrsExprPropDef.Name}:");

            for (var ix = 0; ix < instrs.Length && exp != Exp.None; ix++)
            {
                var i = instrs[ix];

                WriteLineIfDebug($"### {i}");

                try
                {
                    if (skip > 0)
                    {
                        skip--;
                        continue;
                    }

                    if (Expecting(Exp.ExprArrA))
                    {
                        if (i.OpCode.Code == Code.Newarr
                            && i.Operand is TypeReference attributeRef
                            && attributeRef.Name.EndsWith("Attribute"))
                        {
                            skip = 2; // skip: dup, ldc.i4.0
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
                            NewCurrTgtAttrIfNull();
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
                            skip = 2; // skip: dup, ldc.i4.0
                            exp = Exp.TgtAttrNewObj
                                | Exp.TgtCtorArgBinding_Src
                                | Exp.ExprArrZ;
                            continue;
                        }
                    }

                    if (Expecting(Exp.TgtPropBinding_Src))
                    {
                        // From Field mapped in Ctor
                        if (i.OpCode.Code == Code.Ldfld
                            && i.Operand is FieldReference srcFieldRef)
                        {
                            currMembBinding = new MembBinding
                            {
                                SrcKind = SrcKind.FieldMappedInCtor,
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
                            var hasDefault = TryGetPropDefaultValue(srcPropDef, out var srcPropDefaultValue, out var srcPropDefaultTypeRef);
                            currMembBinding = new MembBinding
                            {
                                SrcKind = SrcKind.Property,
                                SrcPropDef = srcPropDef,
                                SrcPropHasDefault = hasDefault,
                                SrcPropDefaultValue = srcPropDefaultValue,
                                SrcPropDefaultTypeRef = srcPropDefaultTypeRef,
                                TgtKind = TgtKind.Property,
                            };
                            currTgtAttr.MembBindings.Add(currMembBinding);
                            exp = Exp.TgtPropBinding_Tgt;
                            continue;
                        }

                        // From Constant/Literal Number
                        if (IsLdcOpCodeInstr(i) && i.Next.OpCode.Code != Code.Newarr)
                        {
                            if (IsConvOpCodeInstr(i.Next))
                                skip = 1;

                            currMembBinding = new MembBinding
                            {
                                SrcKind = SrcKind.Constant,
                                SrcConst = ValueFromLdcInstr(i),
                                TgtKind = TgtKind.Property
                            };
                            currTgtAttr.MembBindings.Add(currMembBinding);
                            exp = Exp.TgtPropBinding_Tgt;
                            continue;
                        }

                        // From Constant/Literal String
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

                        // From Literal Array
                        if (IsLdcOpCodeInstr(i) && i.Next.OpCode.Code == Code.Newarr)
                        {
                            var (arr, instrsHandled) = GetLiteralArray(i.Next);
                            skip += instrsHandled;

                            currMembBinding = new MembBinding
                            {
                                SrcKind = SrcKind.Constant,
                                SrcConst = arr,
                                TgtKind = TgtKind.Property
                            };
                            currTgtAttr.MembBindings.Add(currMembBinding);
                            exp = Exp.TgtPropBinding_Tgt;
                            continue;
                        }

                        // From Literal Type
                        if (i.OpCode.Code == Code.Ldtoken && i.Operand is TypeReference srcTypeRef)
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
                        // From Field mapped in Ctor
                        if (i.OpCode.Code == Code.Ldfld
                            && i.Operand is FieldReference srcFieldRef)
                        {
                            NewCurrTgtAttrIfNull();

                            currTgtAttr.MembBindings.Add(new MembBinding
                            {
                                SrcKind = SrcKind.FieldMappedInCtor,
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
                            NewCurrTgtAttrIfNull();

                            var srcPropDef = srcGetterRef.DeclaringType.Resolve().Properties.First(p => p.GetMethod.FullName == srcGetterRef.FullName);
                            var hasDefault = TryGetPropDefaultValue(srcPropDef, out var srcPropDefaultValue, out var srcPropDefaultTypeRef);
                            currTgtAttr.MembBindings.Add(new MembBinding
                            {
                                SrcKind = SrcKind.Property,
                                SrcPropDef = srcPropDef,
                                SrcPropHasDefault = hasDefault,
                                SrcPropDefaultValue = srcPropDefaultValue,
                                SrcPropDefaultTypeRef = srcPropDefaultTypeRef,
                                TgtKind = TgtKind.CtorArg,
                            });
                            exp = Exp.TgtCtorArgBinding_Src
                                | Exp.TgtAttrNewObj;
                            continue;
                        }

                        // From Constant/Literal Number
                        if (IsLdcOpCodeInstr(i) && i.Next.OpCode.Code != Code.Newarr)
                        {
                            NewCurrTgtAttrIfNull();

                            if (IsConvOpCodeInstr(i.Next))
                                skip = 1;

                            currTgtAttr.MembBindings.Add(new MembBinding
                            {
                                SrcKind = SrcKind.Constant,
                                SrcConst = ValueFromLdcInstr(i),
                                TgtKind = TgtKind.CtorArg,
                            });
                            exp = Exp.TgtCtorArgBinding_Src
                                | Exp.TgtAttrNewObj;
                            continue;
                        }

                        // From Constant/Literal String
                        if (i.OpCode.Code == Code.Ldstr)
                        {
                            NewCurrTgtAttrIfNull();

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

                        // From Literal Array
                        if (IsLdcOpCodeInstr(i) && i.Next.OpCode.Code == Code.Newarr)
                        {
                            NewCurrTgtAttrIfNull();

                            var (arr, instrsHandled) = GetLiteralArray(i.Next);
                            skip += instrsHandled;

                            currTgtAttr.MembBindings.Add(new MembBinding
                            {
                                SrcKind = SrcKind.Constant,
                                SrcConst = arr,
                                TgtKind = TgtKind.CtorArg,
                            });

                            exp = Exp.TgtCtorArgBinding_Src
                                | Exp.TgtAttrNewObj;
                            continue;
                        }

                        // From Literal Type
                        if (i.OpCode.Code == Code.Ldtoken && i.Operand is TypeReference srcTypeRef)
                        {
                            NewCurrTgtAttrIfNull();

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

            return currMarkerImpl;
        }

        public override IEnumerable<string> GetAssembliesForScanning()
        {
            yield return "netstandard";
            yield return "mscorlib";
            yield return "SurrogateAttribute.Core";
        }

        public override bool ShouldCleanReference => true;

        static bool IsLdcOpCodeInstr(Instruction instr)
        {
            var opCode = instr.OpCode;
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

        static bool IsConvOpCodeInstr(Instruction instr)
        {
            var opCode = instr.OpCode;
            return opCode.Code == Code.Conv_I1
                || opCode.Code == Code.Conv_I2
                || opCode.Code == Code.Conv_I4
                || opCode.Code == Code.Conv_I8;
        }

        static object ValueFromLdcInstr(Instruction i)
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

        static bool TryGetPropDefaultValue(PropertyDefinition propDef, out object value, out TypeReference typeRef)
        {
            do
            {
                if (!propDef.HasCustomAttributes)
                    break;

                var propDefaultValueAttr = propDef.CustomAttributes.FirstOrDefault(a => a.AttributeType.FullName == "SurrogateAttribute.PropertyDefaultValueAttribute");
                if (propDefaultValueAttr == null)
                    break;

                var customAttrArgument = propDefaultValueAttr.ConstructorArguments[0];
                if (propDef.PropertyType.FullName != customAttrArgument.Type.FullName)
                    throw new WeavingException($"'PropertyDefaultValueAttribute({customAttrArgument.Type.Name})' does not match its property '{propDef.PropertyType.Name} {propDef.Name}'.") { SequencePoint = propDef.GetMethod?.DebugInformation.SequencePoints[0] };

                value = customAttrArgument.Value;
                typeRef = customAttrArgument.Type;

                return true;
            }
            while (false);

            value = null;
            typeRef = null;
            return false;
        }

        static bool TryGetValueMappedToFieldInCtor(CustomAttribute attr, FieldReference fieldRef, Usage usage, out object value, out TypeReference typeRef)
        {
            do
            {
                var ctorDef = attr.Constructor.Resolve();

                if (!ctorDef.HasBody)
                    break;

                var instrs = ctorDef.Body.Instructions;
                foreach (var i in instrs)
                {
                    if (i.OpCode.Code == Code.Stfld
                        && i.Operand is FieldReference fieldRef2
                        && fieldRef == fieldRef2)
                    {
                        // From Ctor arg
                        if (ctorDef.HasParameters && IsLdargOpCode(i.Previous.OpCode))
                        {
                            int attrCtorArgIx;

                            // Operand can be either ctor arg index or name.
                            var operandStr = i.Previous.Operand?.ToString();
                            if (operandStr != null && operandStr.Length > 0 && !char.IsDigit(operandStr[0]))
                                attrCtorArgIx = attr.Constructor.Parameters.First(p => p.Name == operandStr).Index;
                            else
                                attrCtorArgIx = ValueFromLdargInstruction(i.Previous) - 1;

                            var attrCtorArg = attr.ConstructorArguments.ElementAt(attrCtorArgIx);

                            value = attrCtorArg.Value;
                            typeRef = attrCtorArg.Type;
                            return true;
                        }

                        // From Constant/Literal Number
                        else if (IsLdcOpCodeInstr(i.Previous))
                        {
                            value = ValueFromLdcInstr(i.Previous);
                            typeRef = usage.ModuleDef.ImportReference(value.GetType());
                            return true;
                        }
                        else if (IsConvOpCodeInstr(i.Previous) && IsLdcOpCodeInstr(i.Previous.Previous))
                        {
                            value = ValueFromLdcInstr(i.Previous.Previous);
                            typeRef = usage.ModuleDef.ImportReference(value.GetType());
                            return true;
                        }

                        // From Constant/Literal string
                        else if (i.Previous.OpCode.Code == Code.Ldstr)
                        {
                            value = (string)i.Previous.Operand;
                            typeRef = usage.ModuleDef.ImportReference(value.GetType());
                            return true;
                        }

                        // From Literal Type
                        else if (i.Previous.OpCode.Code == Code.Ldtoken
                            && i.Previous.Operand is TypeReference srcTypeRef)
                        {
                            value = srcTypeRef;
                            typeRef = usage.ModuleDef.ImportReference(value.GetType());
                            return true;
                        }

                        else
                        {
                            throw new WeavingException($"Cannot use complex operations when assigning a value to '{fieldRef.Name}' in the constructor of '{fieldRef.DeclaringType.Name}'.") { SequencePoint = ctorDef.DebugInformation.SequencePoints[0] };
                        }
                    }
                }

            }
            while (false);

            value = null;
            typeRef = null;
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

        static (IList Arr, int InstrsHandled) GetLiteralArray(Instruction newarrInstr)
        {
            var elTypeName = ((TypeReference)newarrInstr.Operand).FullName;
            var elType = elTypeName == "System.Type" ? typeof(TypeReference) : Type.GetType(elTypeName);
            var len = (int)ValueFromLdcInstr(newarrInstr.Previous);
            var arr = Array.CreateInstance(elType, len);
            var ix = 0;
            var skip = 0;
            var instrsHandled = 1;
            for (var i = newarrInstr.Next; i.Next != null; i = i.Next)
            {
                if (i.OpCode.Code == Code.Callvirt)
                    return (arr, instrsHandled - 1);
                if (i.Next.OpCode.Code == Code.Newarr)
                    return (arr, instrsHandled - 1);

                instrsHandled++;

                if (skip > 0)
                {
                    skip--;
                    continue;
                }

                if (i.OpCode.Code == Code.Dup)
                {
                    if (IsLdcOpCodeInstr(i.Next))
                    {
                        skip = 1;
                        continue;
                    }
                    else if (i.Next.OpCode.Code == Code.Ldtoken
                        && i.Next.Operand is FieldReference f
                        && f.DeclaringType.FullName == "<PrivateImplementationDetails>")
                    {
                        skip += 1;
                        var arrBytes = (i.Next.Operand as FieldReference).Resolve().InitialValue;
                        int elSize;
                        if (elType == typeof(byte) || elType == typeof(sbyte) || elType == typeof(bool))
                            elSize = 1;
                        else if (elType == typeof(short) || elType == typeof(ushort) || elType == typeof(char))
                            elSize = 2;
                        else if (elType == typeof(int) || elType == typeof(uint) || elType == typeof(float))
                            elSize = 4;
                        else if (elType == typeof(long) || elType == typeof(ulong) || elType == typeof(double))
                            elSize = 8;
                        else if (elType == typeof(decimal))
                            elSize = 16;
                        else
                            throw new Exception($"Unexpected element type '{elType}' when parsing an optimized array argument.");

                        Buffer.BlockCopy(arrBytes, 0, arr, 0, len * elSize);
                        break;
                    }
                    else
                    {
                        throw new Exception($"Unexpected OpCode '{i.Next.OpCode.Code}' when parsing an array argument.");
                    }
                }

                object el;

                // From Constant/Literal Number
                if (IsLdcOpCodeInstr(i) && i.Next.OpCode.Code != Code.Newarr)
                {
                    el = ValueFromLdcInstr(i);

                    if (IsConvOpCodeInstr(i.Next))
                        skip += 1;

                    if (i.Next.OpCode.Code == Code.Box)
                    {
                        skip += 1;
                        el = Convert.ChangeType(el, Type.GetType(i.Next.Operand.ToString()));
                    }
                    else
                        el = Convert.ChangeType(el, elType);
                }

                // From Constant/Literal String
                else if (i.OpCode.Code == Code.Ldstr)
                {
                    el = (string)i.Operand;
                }

                // From Literal Type
                else if (i.OpCode.Code == Code.Ldtoken && i.Operand is TypeReference srcTypeRef)
                {
                    el = srcTypeRef;
                    skip += 1; // call System.Type System.Type::GetTypeFromHandle
                }

                else
                {
                    throw new Exception($"Unexpected OpCode '{i.OpCode.Code}' when parsing an array argument.");
                }

                arr.SetValue(el, ix++);

                skip += 1; // stelem.ref

                if (ix == len)
                    break;
            }

            instrsHandled += skip;

            return (arr, instrsHandled);
        }

        static void WriteLineIfDebug(string value)
        {
#if DEBUG
            Console.WriteLine(value);
#endif
        }

    }
}
