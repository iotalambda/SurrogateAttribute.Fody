//#define ENABLE_THIS

using System;

namespace TestAssembly;

public static class CtorMappingTypes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SourceAttribute : Attribute
#if ENABLE_THIS || ENABLE_ALL
        , SurrogateAttribute.ISurrogateAttribute
#endif
    {
        public Attribute[] TargetAttributes =>
            [new TargetAttribute(
                boolArgField,
                byteArgField,
                charArgField,
                doubleArgField,
                enumArgField,
                floatArgField,
                intArgField,
                longArgField,
                stringArgField,
                typeArgField)];

        bool boolArgField;
        byte byteArgField;
        char charArgField;
        double doubleArgField;
        MyEnum enumArgField;
        float floatArgField;
        int intArgField;
        long longArgField;
        string stringArgField;
        Type typeArgField;

        public SourceAttribute(Type typeArg)
        {
            boolArgField = Values.BoolArg;
            byteArgField = Values.ByteArg;
            charArgField = Values.CharArg;
            doubleArgField = Values.DoubleArg;
            enumArgField = Values.EnumArg;
            floatArgField = Values.FloatArg;
            intArgField = Values.IntArg;
            longArgField = Values.LongArg;
            stringArgField = Values.StringArg;
            typeArgField = typeArg;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class TargetAttribute : Attribute
    {
        public TargetAttribute(
            bool boolArg,
            byte byteArg,
            char charArg,
            double doubleArg,
            MyEnum enumArg,
            float floatArg,
            int intArg,
            long longArg,
            string stringArg,
            Type typeArg)
        {
            BoolArg = boolArg;
            ByteArg = byteArg;
            CharArg = charArg;
            DoubleArg = doubleArg;
            EnumArg = enumArg;
            FloatArg = floatArg;
            IntArg = intArg;
            LongArg = longArg;
            StringArg = stringArg;
            TypeArg = typeArg;
        }

        public bool BoolArg { get; }
        public byte ByteArg { get; }
        public char CharArg { get; }
        public double DoubleArg { get; }
        public MyEnum EnumArg { get; }
        public float FloatArg { get; }
        public int IntArg { get; }
        public long LongArg { get; }
        public string StringArg { get; }
        public Type TypeArg { get; }
    }

    public class Class
    {
        [Source(typeof(TypeArgValue))]
        public string ClassProp { get; set; }
    }

    public static class Values
    {
        public const bool BoolArg = true;
        public const byte ByteArg = 4;
        public const char CharArg = 'X';
        public const double DoubleArg = -44400.0;
        public const MyEnum EnumArg = MyEnum.ArgValue;
        public const float FloatArg = -.0023F;
        public const int IntArg = -1;
        public const long LongArg = 1;
        public const string StringArg = "";
        public static readonly Type TypeArg = typeof(TypeArgValue);
    }

    public class TypeArgValue;

    public enum MyEnum { ArgValue, PropValue }
}
