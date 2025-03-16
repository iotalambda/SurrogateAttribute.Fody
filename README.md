[![NuGet version (SurrogateAttribute.Fody)](https://img.shields.io/nuget/v/SurrogateAttribute.Fody.svg?style=flat)](https://www.nuget.org/packages/SurrogateAttribute.Fody/) [![Build, Test and Deploy to NuGet.org](https://github.com/iotalambda/SurrogateAttribute.Fody/actions/workflows/main.yml/badge.svg)](https://github.com/iotalambda/SurrogateAttribute.Fody/actions/workflows/main.yml)

![Icon](https://raw.githubusercontent.com/iotalambda/SurrogateAttribute.Fody/main/icon.png)


A Fody add-in that allows creating C# attributes composed of other attributes, thus making C#'s attributes a bit more useful.

## In a nutshell

Instead of having **a mess** like this:

```c#
class Cat
{
    [Required(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = nameof(Translations.NameRequired))]
    [MinLength(3, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = nameof(Translations.NameBadLength))]
    [MaxLength(10, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = nameof(Translations.NameBadLength))]
    [DisplayAttribute(ResourceType = typeof(Translations), Name = nameof(Translations.CatName))]
    public string Name { get; set; }
}
```

You can have:

```c#
class Cat
{
    [RequiredWithLength(MinLength = 3)]
    [Translation(nameof(Translations.CatName))]
    public string Name { get; set; }
}
```

By creating **custom attributes** that implement `ISurrogateAttribute`:

```c#
[AttributeUsage(AttributeTargets.Property)]
class RequiredWithLengthAttribute : Attribute, ISurrogateAttribute
{
    [PropertyDefaultValue(1)]
    public int MinLength { get; set; }

    [PropertyDefaultValue(10)]
    public int MaxLength { get; set; }

    [PropertyDefaultValue(typeof(Translations))]
    Type TranslationResourceType { get; }

    Attribute[] ISurrogateAttribute.TargetAttributes => [
        new RequiredAttribute { ErrorMessageResourceType = TranslationResourceType, ErrorMessageResourceName = nameof(Translations.NameRequired) },
        new MinLengthAttribute(MinLength) { ErrorMessageResourceType = TranslationResourceType, ErrorMessageResourceName = nameof(Translations.NameBadLength) },
        new MaxLengthAttribute(MaxLength) { ErrorMessageResourceType = TranslationResourceType, ErrorMessageResourceName = nameof(Translations.NameBadLength) },
    ];
}

[AttributeUsage(AttributeTargets.Property)]
class TranslationAttribute(string translationKey) : Attribute, ISurrogateAttribute
{
    Attribute[] ISurrogateAttribute.TargetAttributes => [
        new DisplayAttribute { ResourceType = typeof(Translations), Name = translationKey }
    ];
}
```

Usages of those custom attributes are **replaced** by their corresponding `TargetAttributes` at build time using IL weaving.

## Installation

Install [Fody](https://github.com/Fody/Fody) and SurrogateAttribute.Fody to each project in which you want to _use_ surrogate attributes:
```powershell
Install-Package Fody
Install-Package SurrogateAttribute.Fody
```

And make sure both dependencies have `PrivateAssets="All"` like so, because they are only needed during build:
<!--PACKAGEREFERENCES-->
```xml
<PackageReference Include="Fody" Version="6.8.1" PrivateAssets="All" />
<PackageReference Include="SurrogateAttribute.Fody" Version="0.6.7" PrivateAssets="All" />
```

A `FodyWeavers.xml` file will be added automatically to the project on rebuild. If not, create the file with the following content:
```xml
<Weavers>
  <SurrogateAttribute />
</Weavers>
```

This includes `SurrogateAttribute` Fody add-in to the IL weaving process.

In case your project (most likely a library) _has no_ surrogate attribute usages but _has_ surrogate attribute implementations, you may want to install `SurrogateAttribute.Core` instead, which has the required types for implementing surrogate attributes but does not include `Fody` as a dependency:
```powershell
Install-Package SurrogateAttribute.Core
```

See [Samples](https://github.com/iotalambda/SurrogateAttribute.Fody/tree/main/Samples) for a working solution.

## Features

<!--SURROGATEATTRIBUTEEXAMPLE-->
```c#
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
```

### Supported targets for surrogate attributes

The following table displays what's currently supported. The support will be extended to cover all targets in the future:

| `AttributeTarget` | Supported currently? |
|:-|:-:|
| `Assembly` | 🚫 |
| `Module` | 🚫 |
| `Class` | ✅ |
| `Struct` | 🚫 |
| `Enum` | 🚫 |
| `Constructor` | 🚫 |
| `Method` | 🚫 |
| `Property` | ✅ |
| `Field` | 🚫 |
| `Event` | 🚫 |
| `Interface` | ✅ |
| `Parameter` | 🚫 |
| `Delegate` | 🚫 |
| `ReturnValue` | 🚫 |
| `GenericParameter` | 🚫 |

