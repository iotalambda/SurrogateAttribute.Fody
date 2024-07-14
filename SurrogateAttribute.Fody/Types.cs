using Mono.Cecil;
using System;
using System.Collections.Generic;

namespace SurrogateAttribute.Fody
{
    [Flags]
    enum Exp
    {
        None = 0,
        ExprArrA = 1 << 0,
        ExprArrZ = 1 << 1,
        TgtAttrNewObj = 1 << 2,
        TgtPropBinding_Src = 1 << 3,
        TgtPropBinding_Tgt = 1 << 4,
        TgtCtorArgBinding_Src = 1 << 5,
        TgtAttrZ = 1 << 6,
    }

    class MarkerImpl
    {
        public TypeDefinition TypeDef { get; set; }
        public List<TgtAttr> TgtAttrs { get; set; } = new List<TgtAttr>();
    }

    class TgtAttr
    {
        public MethodReference CtorRef { get; set; }
        public TypeReference TypeRef { get; set; }
        public List<MembBinding> MembBindings { get; } = new List<MembBinding>();
    }

    class MembBinding
    {
        public SrcKind SrcKind { get; set; }
        public PropertyDefinition SrcPropDef { get; set; }
        public bool SrcPropHasDefault { get; set; }
        public object SrcPropDefault { get; set; }
        public object SrcConst { get; set; }
        public FieldReference SrcFieldRef { get; set; }

        public TgtKind TgtKind { get; set; }
        public PropertyDefinition TgtPropDef { get; set; }
    }

    enum SrcKind
    {
        Property = 1,
        Constant = 2,
        FieldMappedFromCtorArg = 3,
    }

    enum TgtKind
    {
        Property = 1,
        CtorArg = 3,
    }

    abstract class Usage
    {
        public ICustomAttributeProvider CustAttrProvider { get; set; }
        public CustomAttribute Attr { get; set; }
        public TypeDefinition AttrTypeDef { get; set; }
        public ModuleDefinition ModuleDef { get; set; }
    }
    class ClassInterfUsage : Usage { }
    class PropUsage : Usage { }
}
