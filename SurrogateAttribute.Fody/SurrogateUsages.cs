using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SurrogateAttribute.Fody
{
    static class SurrogateUsages
    {
        public static bool TryGetClassInterfUsages(TypeDefinition targetTypeDef, TypeDefinition markerTypeDef, in List<Usage> usages)
        {
            try
            {
                var any = false;
                if ((targetTypeDef.IsClass || targetTypeDef.IsInterface) && targetTypeDef.HasCustomAttributes)
                {
                    foreach (var targetTypeAttr in targetTypeDef.CustomAttributes)
                    {
                        if (!TryGetSurrogateAttrTypeDef(targetTypeAttr, markerTypeDef, out var targetTypeAttrTypeDef))
                            continue;

                        usages.Add(new ClassInterfUsage
                        {
                            CustAttrProvider = targetTypeDef,
                            Attr = targetTypeAttr,
                            AttrTypeDef = targetTypeAttrTypeDef,
                            ModuleDef = targetTypeDef.Module,
                        });
                        any = true;
                    }
                }
                return any;
            }
            catch (Exception e)
            {
                throw e.EnsureWeavingException($"An exception was thrown when trying to get class/interface surrogate attributes from the type '{targetTypeDef.Name}'.");
            }
        }

        public static bool TryGetPropUsages(TypeDefinition targetTypeDef, TypeDefinition markerTypeDef, in List<Usage> usages)
        {
            try
            {
                var any = false;
                if (targetTypeDef.HasProperties)
                {
                    foreach (var targetTypePropDef in targetTypeDef.Properties)
                    {
                        try
                        {
                            if (targetTypePropDef.HasCustomAttributes)
                            {
                                foreach (var targetTypePropAttr in targetTypePropDef.CustomAttributes)
                                {
                                    if (!TryGetSurrogateAttrTypeDef(targetTypePropAttr, markerTypeDef, out var targetTypePropAttrTypeDef))
                                        continue;

                                    usages.Add(new PropUsage
                                    {
                                        CustAttrProvider = targetTypePropDef,
                                        Attr = targetTypePropAttr,
                                        AttrTypeDef = targetTypePropAttrTypeDef,
                                        ModuleDef = targetTypeDef.Module,
                                    });

                                    any = true;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            throw e.EnsureWeavingException(
                                $"An exception was thrown when trying to get surrogate attributes from the property '{targetTypePropDef.Name}' of the type '{targetTypeDef.Name}'.",
                                targetTypePropDef.GetMethod?.DebugInformation.SequencePoints[0]);
                        }
                    }
                }
                return any;
            }
            catch (Exception e)
            {
                throw e.EnsureWeavingException($"An exception was throw when trying to get property surrogate attributes from the type '{targetTypeDef.Name}'.");
            }
        }

        static bool TryGetSurrogateAttrTypeDef(CustomAttribute attr, TypeDefinition markerTypeDef, out TypeDefinition attrTypeDef)
        {
            attrTypeDef = attr.AttributeType.Resolve();
            if (!attrTypeDef.HasInterfaces)
                return false;

            var attrMarkerImpl = attrTypeDef.Interfaces.FirstOrDefault(i => i.InterfaceType.FullName == markerTypeDef.FullName);
            if (attrMarkerImpl == null)
                return false;

            return true;
        }
    }
}
