//#define ENABLE_THIS

using System;

namespace TestAssembly;

public static class Types
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SourceAttribute : Attribute
#if ENABLE_THIS || ENABLE_ALL
        , SurrogateAttribute.ISurrogateAttribute
#endif
    {
        public Attribute[] TargetAttributes =>
            [new TargetAttribute
            (
                Values.BoolArg,
                Values.ByteArg,
                Values.CharArg,
                Values.DoubleArg,
                Values.EnumArg,
                Values.FloatArg,
                Values.IntArg,
                Values.LongArg,
                Values.StringArg,
                typeof(TypeArgValue)
            )
            {
                BoolProp = Values.BoolProp,
                ByteProp = Values.ByteProp,
                CharProp = Values.CharProp,
                DoubleProp = Values.DoubleProp,
                EnumProp = Values.EnumProp,
                FloatProp = Values.FloatProp,
                IntProp = Values.IntProp,
                LongProp = Values.LongProp,
                StringProp = Values.StringProp,
                TypeProp = typeof(DateTime),
            }];
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

        public bool BoolArg { get; set; }
        public bool BoolProp { get; set; }
        public byte ByteArg { get; set; }
        public byte ByteProp { get; set; }
        public char CharArg { get; set; }
        public char CharProp { get; set; }
        public double DoubleArg { get; set; }
        public double DoubleProp { get; set; }
        public MyEnum EnumArg { get; set; }
        public MyEnum EnumProp { get; set; }
        public float FloatArg { get; set; }
        public float FloatProp { get; set; }
        public int IntArg { get; set; }
        public int IntProp { get; set; }
        public long LongArg { get; set; }
        public long LongProp { get; set; }
        public string StringArg { get; set; }
        public string StringProp { get; set; }
        public Type TypeArg { get; set; }
        public Type TypeProp { get; set; }
    }

    public class Class
    {
        [Source]
        public object ClassProp { get; set; }
    }

    public static class Values
    {
        public const bool BoolArg = true;
        public const bool BoolProp = true;
        public const byte ByteArg = 4;
        public const byte ByteProp = 5;
        public const char CharArg = 'X';
        public const char CharProp = 'C';
        public const double DoubleArg = -44400.0;
        public const double DoubleProp = 4.56;
        public const MyEnum EnumArg = MyEnum.ArgValue;
        public const MyEnum EnumProp = MyEnum.PropValue;
        public const float FloatArg = -.0023F;
        public const float FloatProp = 1.23F;
        public const int IntArg = -1;
        public const int IntProp = 123;
        public const long LongArg = 1;
        public const long LongProp = 456;
        public const string StringArg = "";
        public const string StringProp = "Str";
        public static readonly Type TypeArg = typeof(TypeArgValue);
        public static readonly Type TypeProp = typeof(DateTime);
    }

    public class TypeArgValue;

    public enum MyEnum { ArgValue, PropValue }
}