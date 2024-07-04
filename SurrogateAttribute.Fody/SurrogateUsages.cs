using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;

namespace SurrogateAttribute.Fody
{
    static class SurrogateUsages
    {
        public static bool TryGetClassInterfUsages(TypeDefinition targetTypeDef, TypeDefinition markerTypeDef, in List<Usage> usages)
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
                    });
                    any = true;
                }
            }
            return any;
        }

        public static bool TryGetPropUsages(TypeDefinition targetTypeDef, TypeDefinition markerTypeDef, in List<Usage> usages)
        {
            var any = false;
            if (targetTypeDef.HasProperties)
            {
                foreach (var targetTypePropDef in targetTypeDef.Properties)
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
                            });

                            any = true;
                        }
                    }
                }
            }
            return any;
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
