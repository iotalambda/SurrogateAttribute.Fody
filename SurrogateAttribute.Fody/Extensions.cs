using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System;

namespace SurrogateAttribute.Fody
{
    internal static class Extensions
    {
        public static bool InheritsFrom(this TypeDefinition typeDef, TypeDefinition baseTypeDef)
        {
            if (typeDef.BaseType == null)
                return false;

            var nextTypeDef = typeDef.BaseType.Resolve();
            if (nextTypeDef == baseTypeDef)
                return true;

            return InheritsFrom(nextTypeDef, baseTypeDef);
        }

        public static Exception EnsureWeavingException(this Exception e, string errorMessage, SequencePoint sp = null)
        {
            var we = e as WeavingException;
            if (we == null)
                we = new WeavingException($"{errorMessage}{Environment.NewLine}{e.Message}");
            if (we.SequencePoint == null)
                we.SequencePoint = sp;
            return we;
        }
    }
}
