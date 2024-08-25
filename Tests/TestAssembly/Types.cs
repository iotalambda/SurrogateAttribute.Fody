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
                [6, 5, 4],
                ["A_arg", "B_arg", "C_arg"],
                [typeof(short), typeof(int), typeof(long)],
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
                ArrayIntProp = [9, 8, 7],
                ArrayStringProp = ["A_prop", "B_prop", "C_prop"],
                ArrayTypeProp = [typeof(ushort), typeof(uint), typeof(ulong)],
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
            int[] arrayIntArg,
            string[] arrayStringArg,
            Type[] arrayTypeArg,
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
            ArrayIntArg = arrayIntArg;
            ArrayStringArg = arrayStringArg;
            ArrayTypeArg = arrayTypeArg;
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

        public int[] ArrayIntArg { get; set; }
        public int[] ArrayIntProp { get; set; }
        public string[] ArrayStringArg { get; set; }
        public string[] ArrayStringProp { get; set; }
        public Type[] ArrayTypeArg { get; set; }
        public Type[] ArrayTypeProp { get; set; }
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
        public static readonly int[] ArrayIntArg = [6, 5, 4];
        public static readonly int[] ArrayIntProp = [9, 8, 7];
        public static readonly string[] ArrayStringArg = ["A_arg", "B_arg", "C_arg"];
        public static readonly string[] ArrayStringProp = ["A_prop", "B_prop", "C_prop"];
        public static readonly Type[] ArrayTypeArg = [typeof(short), typeof(int), typeof(long)];
        public static readonly Type[] ArrayTypeProp = [typeof(ushort), typeof(uint), typeof(ulong)];
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
        public const long LongArg = 22;
        public const long LongProp = 456;
        public const string StringArg = "asdf";
        public const string StringProp = "Str";
        public static readonly Type TypeArg = typeof(TypeArgValue);
        public static readonly Type TypeProp = typeof(DateTime);
    }

    public class TypeArgValue;

    public enum MyEnum { ArgValue, PropValue }
}