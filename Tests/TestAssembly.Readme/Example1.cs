using SurrogateAttribute;
using System;

namespace TestAssembly.Readme;

public static class Example1
{
    // SURROGATEATTRIBUTEEXAMPLE_START
    [AttributeUsage(AttributeTargets.Class)]
    public class ExampleSurrogateAttribute : Attribute, ISurrogateAttribute
    {
        public ExampleSurrogateAttribute(

            /*
             * Values can be passed to surrogate attributes
             * as ctor arguments:
             */
            bool ctorArg1,
            byte ctorArg2,
            char ctorArg3,
            double ctorArg5,
            float ctorArg6,
            int ctorArg7,
            long ctorArg8,
            sbyte ctorArg9,
            short ctorArg10,
            string ctorArg11,
            Type ctorArg12,
            uint ctorArg13,
            ulong ctorArg14,
            ushort ctorArg15,

            /*
             * Array ctor arguments are also supported:
             */
            bool[] ctorArg16,
            byte[] ctorArg17,
            char[] ctorArg18,
            double[] ctorArg20,
            float[] ctorArg21,
            int[] ctorArg22,
            long[] ctorArg23,
            object[] ctorArg24,
            sbyte[] ctorArg25,
            short[] ctorArg26,
            string[] ctorArg27,
            uint[] ctorArg29,
            ulong[] ctorArg30,
            ushort[] ctorArg31)
        {
            /*
             * Ctor arguments can be stored to fields:
             * 
             * NOTE! Any other complex operations in surrogate
             *       attribute ctors are not supported.
             */
            this.ctorArg1 = ctorArg1;
            this.ctorArg2 = ctorArg2;
            this.ctorArg3 = ctorArg3;
            this.ctorArg5 = ctorArg5;
            this.ctorArg6 = ctorArg6;
            this.ctorArg7 = ctorArg7;
            this.ctorArg8 = ctorArg8;
            this.ctorArg9 = ctorArg9;
            this.ctorArg10 = ctorArg10;
            this.ctorArg11 = ctorArg11;
            this.ctorArg12 = ctorArg12;
            this.ctorArg13 = ctorArg13;
            this.ctorArg14 = ctorArg14;
            this.ctorArg15 = ctorArg15;
            this.ctorArg16 = ctorArg16;
            this.ctorArg17 = ctorArg17;
            this.ctorArg18 = ctorArg18;
            this.ctorArg20 = ctorArg20;
            this.ctorArg21 = ctorArg21;
            this.ctorArg22 = ctorArg22;
            this.ctorArg23 = ctorArg23;
            this.ctorArg24 = ctorArg24;
            this.ctorArg25 = ctorArg25;
            this.ctorArg26 = ctorArg26;
            this.ctorArg27 = ctorArg27;
            this.ctorArg29 = ctorArg29;
            this.ctorArg30 = ctorArg30;
            this.ctorArg31 = ctorArg31;
        }

        private readonly bool ctorArg1;
        private readonly byte ctorArg2;
        private readonly char ctorArg3;
        private readonly double ctorArg5;
        private readonly float ctorArg6;
        private readonly int ctorArg7;
        private readonly long ctorArg8;
        private readonly sbyte ctorArg9;
        private readonly short ctorArg10;
        private readonly string ctorArg11;
        private readonly Type ctorArg12;
        private readonly uint ctorArg13;
        private readonly ulong ctorArg14;
        private readonly ushort ctorArg15;
        private readonly bool[] ctorArg16;
        private readonly byte[] ctorArg17;
        private readonly char[] ctorArg18;
        private readonly double[] ctorArg20;
        private readonly float[] ctorArg21;
        private readonly int[] ctorArg22;
        private readonly long[] ctorArg23;
        private readonly object[] ctorArg24;
        private readonly sbyte[] ctorArg25;
        private readonly short[] ctorArg26;
        private readonly string[] ctorArg27;
        private readonly uint[] ctorArg29;
        private readonly ulong[] ctorArg30;
        private readonly ushort[] ctorArg31;

        /*
         * Values can be passed to surrogate attributes
         * as named arguments:
         */
        public bool NamedArg1 { get; set; }
        public byte NamedArg2 { get; set; }
        public char NamedArg3 { get; set; }
        public double NamedArg5 { get; set; }
        public float NamedArg6 { get; set; }
        public int NamedArg7 { get; set; }
        public long NamedArg8 { get; set; }
        public sbyte NamedArg9 { get; set; }
        public short NamedArg10 { get; set; }

        /*
         * Named arguments can be provided default values with
         * the `PropertyDefaultValueAttribute` attribute.
         */
        [PropertyDefaultValue($"Some default value for {nameof(NamedArg11)}")]
        public string NamedArg11 { get; set; }
        public Type NamedArg12 { get; set; }
        public uint NamedArg13 { get; set; }
        public ulong NamedArg14 { get; set; }
        public ushort NamedArg15 { get; set; }
        public bool[] NamedArg16 { get; set; }
        public byte[] NamedArg17 { get; set; }
        public char[] NamedArg18 { get; set; }
        public double[] NamedArg20 { get; set; }
        public float[] NamedArg21 { get; set; }
        public int[] NamedArg22 { get; set; }
        public long[] NamedArg23 { get; set; }
        public object[] NamedArg24 { get; set; }
        public sbyte[] NamedArg25 { get; set; }
        public short[] NamedArg26 { get; set; }
        public string[] NamedArg27 { get; set; }
        public uint[] NamedArg29 { get; set; }
        public ulong[] NamedArg30 { get; set; }
        public ushort[] NamedArg31 { get; set; }

        /*
         * Constants are supported:
         */
        private const bool Constant1 = true;
        private const byte Constant2 = 5;
        private const char Constant3 = 'S';
        private const double Constant5 = 0.3;
        private const float Constant6 = 0.6F;
        private const int Constant7 = 100;
        private const long Constant8 = 1000;
        private const sbyte Constant9 = -4;
        private const short Constant10 = 256;
        private const string Constant11 = "constant string";
        private const uint Constant12 = 512;
        private const ulong Constant13 = 12345678;
        private const ushort Constant14 = 12345;

        public Attribute[] TargetAttributes => [
            /**
             * Target attribute ctor and named arguments can be provided from
             * constants and the surrogate attribute's fields and properties.
             */
            new TargetAttribute(
                ctorArg1,
                ctorArg2,
                ctorArg3,
                ctorArg8,
                ctorArg9,
                ctorArg10,
                ctorArg11,
                ctorArg16,
                ctorArg17,
                ctorArg18,
                ctorArg24,
                ctorArg25,
                ctorArg26,
                ctorArg27,
                NamedArg1,
                NamedArg2,
                NamedArg3,
                NamedArg5,
                NamedArg6,
                NamedArg7,
                NamedArg16,
                NamedArg17,
                NamedArg18,
                NamedArg20,
                NamedArg21,
                NamedArg22,
                NamedArg23,
                Constant1,
                Constant2,
                Constant3,
                Constant5,
                Constant6,
                Constant7,
                /*
                 * Target attribute ctor arguments can be provided inline:
                 */
                inlineCtorArg1: true,
                inlineCtorArg2: 8,
                inlineCtorArg3: 'A',
                inlineCtorArg5: 0.01,
                inlineCtorArg6: 0.011F,
                inlineCtorArg7: 77,
                inlineCtorArg8: 7777777,
                inlineCtorArg16: [true, true, false],
                inlineCtorArg17: [9, 8, 7, 6],
                inlineCtorArg18: ['D', 'E', 'F'],
                inlineCtorArg20: [9995.4, 4.4, 3.4],
                inlineCtorArg21: [3.4F, 4.4F, 9995.4F],
                inlineCtorArg22: [8, 7, 6, -5],
                inlineCtorArg23: [80000000, 700, 600000, -5000])
            {
                CtorArg5 = ctorArg5,
                CtorArg6 = ctorArg6,
                CtorArg7 = ctorArg7,
                CtorArg12 = ctorArg12,
                CtorArg13 = ctorArg13,
                CtorArg14 = ctorArg14,
                CtorArg15 = ctorArg15,
                CtorArg20 = ctorArg20,
                CtorArg21 = ctorArg21,
                CtorArg22 = ctorArg22,
                CtorArg23 = ctorArg23,
                CtorArg29 = ctorArg29,
                CtorArg30 = ctorArg30,
                CtorArg31 = ctorArg31,
                NamedArg8 = NamedArg8,
                NamedArg9 = NamedArg9,
                NamedArg10 = NamedArg10,
                NamedArg11 = NamedArg11,
                NamedArg12 = NamedArg12,
                NamedArg13 = NamedArg13,
                NamedArg14 = NamedArg14,
                NamedArg15 = NamedArg15,
                NamedArg24 = NamedArg24,
                NamedArg25 = NamedArg25,
                NamedArg26 = NamedArg26,
                NamedArg27 = NamedArg27,
                NamedArg29 = NamedArg29,
                NamedArg30 = NamedArg30,
                NamedArg31 = NamedArg31,
                Constant8 = Constant8,
                Constant9 = Constant9,
                Constant10 = Constant10,
                Constant11 = Constant11,
                Constant12 = Constant12,
                Constant13 = Constant13,
                Constant14 = Constant14,
                /*
                 * Target attribute named arguments can be provided inline:
                 */
                InlineNamedArg9 = 42,
                InlineNamedArg10 = -42,
                InlineNamedArg11 = "dddd",
                InlineNamedArg12 = typeof(DateTime),
                InlineNamedArg13 = 43,
                InlineNamedArg14 = 430000000,
                InlineNamedArg15 = 4,
                InlineNamedArg25 = [2, 2, 2, 2, 2, 2],
                InlineNamedArg26 = [0],
                InlineNamedArg27 = ["ABC", "DEF", "GHJ"],
                InlineNamedArg29 = [3, 3, 3, 3],
                InlineNamedArg30 = [30, 30, 30, 30],
                InlineNamedArg31 = [300, 300, 300, 300]
            },
        ];
    }
    // SURROGATEATTRIBUTEEXAMPLE_END

    [AttributeUsage(AttributeTargets.Class)]
    public class TargetAttribute : Attribute
    {
        public TargetAttribute(
            bool ctorArg1,
            byte ctorArg2,
            char ctorArg3,
            long ctorArg8,
            sbyte ctorArg9,
            short ctorArg10,
            string ctorArg11,
            bool[] ctorArg16,
            byte[] ctorArg17,
            char[] ctorArg18,
            object[] ctorArg24,
            sbyte[] ctorArg25,
            short[] ctorArg26,
            string[] ctorArg27,
            bool namedArg1,
            byte namedArg2,
            char namedArg3,
            double namedArg5,
            float namedArg6,
            int namedArg7,
            bool[] namedArg16,
            byte[] namedArg17,
            char[] namedArg18,
            double[] namedArg20,
            float[] namedArg21,
            int[] namedArg22,
            long[] namedArg23,
            bool constant1,
            byte constant2,
            char constant3,
            double constant5,
            float constant6,
            int constant7,
            bool inlineCtorArg1,
            byte inlineCtorArg2,
            char inlineCtorArg3,
            double inlineCtorArg5,
            float inlineCtorArg6,
            int inlineCtorArg7,
            long inlineCtorArg8,
            bool[] inlineCtorArg16,
            byte[] inlineCtorArg17,
            char[] inlineCtorArg18,
            double[] inlineCtorArg20,
            float[] inlineCtorArg21,
            int[] inlineCtorArg22,
            long[] inlineCtorArg23)
        {
        }

        public double CtorArg5 { get; set; }
        public float CtorArg6 { get; set; }
        public int CtorArg7 { get; set; }
        public Type CtorArg12 { get; set; }
        public uint CtorArg13 { get; set; }
        public ulong CtorArg14 { get; set; }
        public ushort CtorArg15 { get; set; }
        public double[] CtorArg20 { get; set; }
        public float[] CtorArg21 { get; set; }
        public int[] CtorArg22 { get; set; }
        public long[] CtorArg23 { get; set; }
        public uint[] CtorArg29 { get; set; }
        public ulong[] CtorArg30 { get; set; }
        public ushort[] CtorArg31 { get; set; }
        public long NamedArg8 { get; set; }
        public sbyte NamedArg9 { get; set; }
        public short NamedArg10 { get; set; }
        public string NamedArg11 { get; set; }
        public Type NamedArg12 { get; set; }
        public uint NamedArg13 { get; set; }
        public ulong NamedArg14 { get; set; }
        public ushort NamedArg15 { get; set; }
        public object[] NamedArg24 { get; set; }
        public sbyte[] NamedArg25 { get; set; }
        public short[] NamedArg26 { get; set; }
        public string[] NamedArg27 { get; set; }
        public uint[] NamedArg29 { get; set; }
        public ulong[] NamedArg30 { get; set; }
        public ushort[] NamedArg31 { get; set; }
        public long Constant8 { get; set; }
        public sbyte Constant9 { get; set; }
        public short Constant10 { get; set; }
        public string Constant11 { get; set; }
        public uint Constant12 { get; set; }
        public ulong Constant13 { get; set; }
        public ushort Constant14 { get; set; }
        public sbyte InlineNamedArg9 { get; set; }
        public short InlineNamedArg10 { get; set; }
        public string InlineNamedArg11 { get; set; }
        public Type InlineNamedArg12 { get; set; }
        public uint InlineNamedArg13 { get; set; }
        public ulong InlineNamedArg14 { get; set; }
        public ushort InlineNamedArg15 { get; set; }
        public sbyte[] InlineNamedArg25 { get; set; }
        public short[] InlineNamedArg26 { get; set; }
        public string[] InlineNamedArg27 { get; set; }
        public uint[] InlineNamedArg29 { get; set; }
        public ulong[] InlineNamedArg30 { get; set; }
        public ushort[] InlineNamedArg31 { get; set; }
    }

    [ExampleSurrogate(
        Values.CtorArg1,
        Values.CtorArg2,
        Values.CtorArg3,
        Values.CtorArg5,
        Values.CtorArg6,
        Values.CtorArg7,
        Values.CtorArg8,
        Values.CtorArg9,
        Values.CtorArg10,
        Values.CtorArg11,
        ctorArg12: typeof(DateTime),
        Values.CtorArg13,
        Values.CtorArg14,
        Values.CtorArg15,
        ctorArg16: [true, true, false],
        ctorArg17: [9, 8, 7, 6],
        ctorArg18: ['D', 'E', 'F'],
        ctorArg20: [9995.4, 4.4, 3.4],
        ctorArg21: [3.4F, 4.4F, 9995.4F],
        ctorArg22: [8, 7, 6, -5],
        ctorArg23: [80000000, 700, 600000, -5000],
        ctorArg24: ["ABC", 432, true],
        ctorArg25: [2, 2, 2, 2, 2, 2],
        ctorArg26: [0],
        ctorArg27: ["ABC", "DEF", "GHJ"],
        ctorArg29: [3, 3, 3, 3],
        ctorArg30: [30, 30, 30, 30],
        ctorArg31: [300, 300, 300, 300],
        NamedArg1 = Values.NamedArg1,
        NamedArg2 = Values.NamedArg2,
        NamedArg3 = Values.NamedArg3,
        NamedArg5 = Values.NamedArg5,
        NamedArg6 = Values.NamedArg6,
        NamedArg7 = Values.NamedArg7,
        NamedArg8 = Values.NamedArg8,
        NamedArg9 = Values.NamedArg9,
        NamedArg10 = Values.NamedArg10,
        //NamedArg11 = Values.NamedArg11,
        NamedArg12 = typeof(DateTime),
        NamedArg13 = Values.NamedArg13,
        NamedArg14 = Values.NamedArg14,
        NamedArg15 = Values.NamedArg15,
        NamedArg16 = [true, true, false],
        NamedArg17 = [9, 8, 7, 6],
        NamedArg18 = ['D', 'E', 'F'],
        NamedArg20 = [9995.4, 4.4, 3.4],
        NamedArg21 = [3.4F, 4.4F, 9995.4F],
        NamedArg22 = [8, 7, 6, -5],
        NamedArg23 = [80000000, 700, 600000, -5000],
        NamedArg24 = ["ABC", 432, true],
        NamedArg25 = [2, 2, 2, 2, 2, 2],
        NamedArg26 = [0],
        NamedArg27 = ["ABC", "DEF", "GHJ"],
        NamedArg29 = [3, 3, 3, 3],
        NamedArg30 = [30, 30, 30, 30],
        NamedArg31 = [300, 300, 300, 300])]
    public class Class;

    public static class Values
    {
        public const bool CtorArg1 = true;
        public const byte CtorArg2 = 8;
        public const char CtorArg3 = 'A';
        public const double CtorArg5 = 0.01;
        public const float CtorArg6 = 0.011F;
        public const int CtorArg7 = 77;
        public const long CtorArg8 = 7777777;
        public const sbyte CtorArg9 = 42;
        public const short CtorArg10 = -42;
        public const string CtorArg11 = "dddd";
        public static readonly Type CtorArg12 = typeof(DateTime);
        public const uint CtorArg13 = 43;
        public const ulong CtorArg14 = 430000000;
        public const ushort CtorArg15 = 4;
        public static readonly bool[] CtorArg16 = [true, true, false];
        public static readonly byte[] CtorArg17 = [9, 8, 7, 6];
        public static readonly char[] CtorArg18 = ['D', 'E', 'F'];
        public static readonly double[] CtorArg20 = [9995.4, 4.4, 3.4];
        public static readonly float[] CtorArg21 = [3.4F, 4.4F, 9995.4F];
        public static readonly int[] CtorArg22 = [8, 7, 6, -5];
        public static readonly long[] CtorArg23 = [80000000, 700, 600000, -5000];
        public static readonly object[] CtorArg24 = ["ABC", 432, true];
        public static readonly sbyte[] CtorArg25 = [2, 2, 2, 2, 2, 2];
        public static readonly short[] CtorArg26 = [0];
        public static readonly string[] CtorArg27 = ["ABC", "DEF", "GHJ"];
        public static readonly uint[] CtorArg29 = [3, 3, 3, 3];
        public static readonly ulong[] CtorArg30 = [30, 30, 30, 30];
        public static readonly ushort[] CtorArg31 = [300, 300, 300, 300];
        public const bool NamedArg1 = true;
        public const byte NamedArg2 = 8;
        public const char NamedArg3 = 'A';
        public const double NamedArg5 = 0.01;
        public const float NamedArg6 = 0.011F;
        public const int NamedArg7 = 77;
        public const long NamedArg8 = 7777777;
        public const sbyte NamedArg9 = 42;
        public const short NamedArg10 = -42;
        public const string NamedArg11DefaultValue = $"Some default value for {nameof(Example1.ExampleSurrogateAttribute.NamedArg11)}";
        public static readonly Type NamedArg12 = typeof(DateTime);
        public const uint NamedArg13 = 43;
        public const ulong NamedArg14 = 430000000;
        public const ushort NamedArg15 = 4;
        public static readonly bool[] NamedArg16 = [true, true, false];
        public static readonly byte[] NamedArg17 = [9, 8, 7, 6];
        public static readonly char[] NamedArg18 = ['D', 'E', 'F'];
        public static readonly double[] NamedArg20 = [9995.4, 4.4, 3.4];
        public static readonly float[] NamedArg21 = [3.4F, 4.4F, 9995.4F];
        public static readonly int[] NamedArg22 = [8, 7, 6, -5];
        public static readonly long[] NamedArg23 = [80000000, 700, 600000, -5000];
        public static readonly object[] NamedArg24 = ["ABC", 432, true];
        public static readonly sbyte[] NamedArg25 = [2, 2, 2, 2, 2, 2];
        public static readonly short[] NamedArg26 = [0];
        public static readonly string[] NamedArg27 = ["ABC", "DEF", "GHJ"];
        public static readonly uint[] NamedArg29 = [3, 3, 3, 3];
        public static readonly ulong[] NamedArg30 = [30, 30, 30, 30];
        public static readonly ushort[] NamedArg31 = [300, 300, 300, 300];
        public const bool Constant1 = true;
        public const byte Constant2 = 5;
        public const char Constant3 = 'S';
        public const double Constant5 = 0.3;
        public const float Constant6 = 0.6F;
        public const int Constant7 = 100;
        public const long Constant8 = 1000;
        public const sbyte Constant9 = -4;
        public const short Constant10 = 256;
        public const string Constant11 = "constant string";
        public const uint Constant12 = 512;
        public const ulong Constant13 = 12345678;
        public const ushort Constant14 = 12345;
        public const bool InlineCtorArg1 = true;
        public const byte InlineCtorArg2 = 8;
        public const char InlineCtorArg3 = 'A';
        public const double InlineCtorArg5 = 0.01;
        public const float InlineCtorArg6 = 0.011F;
        public const int InlineCtorArg7 = 77;
        public const long InlineCtorArg8 = 7777777;
        public const sbyte InlineNamedArg9 = 42;
        public const short InlineNamedArg10 = -42;
        public const string InlineNamedArg11 = "dddd";
        public static readonly Type InlineNamedArg12 = typeof(DateTime);
        public const uint InlineNamedArg13 = 43;
        public const ulong InlineNamedArg14 = 430000000;
        public const ushort InlineNamedArg15 = 4;
        public static readonly bool[] InlineCtorArg16 = [true, true, false];
        public static readonly byte[] InlineCtorArg17 = [9, 8, 7, 6];
        public static readonly char[] InlineCtorArg18 = ['D', 'E', 'F'];
        public static readonly double[] InlineCtorArg20 = [9995.4, 4.4, 3.4];
        public static readonly float[] InlineCtorArg21 = [3.4F, 4.4F, 9995.4F];
        public static readonly int[] InlineCtorArg22 = [8, 7, 6, -5];
        public static readonly long[] InlineCtorArg23 = [80000000, 700, 600000, -5000];
        public static readonly sbyte[] InlineNamedArg25 = [2, 2, 2, 2, 2, 2];
        public static readonly short[] InlineNamedArg26 = [0];
        public static readonly string[] InlineNamedArg27 = ["ABC", "DEF", "GHJ"];
        public static readonly uint[] InlineNamedArg29 = [3, 3, 3, 3];
        public static readonly ulong[] InlineNamedArg30 = [30, 30, 30, 30];
        public static readonly ushort[] InlineNamedArg31 = [300, 300, 300, 300];
    }
}
