[![NuGet version (SurrogateAttribute.Fody)](https://img.shields.io/nuget/v/SurrogateAttribute.Fody.svg?style=flat)](https://www.nuget.org/packages/SurrogateAttribute.Fody/) [![Build, Test and Deploy to NuGet.org](https://github.com/iotalambda/SurrogateAttribute.Fody/actions/workflows/main.yml/badge.svg)](https://github.com/iotalambda/SurrogateAttribute.Fody/actions/workflows/main.yml)

![Icon](https://raw.githubusercontent.com/iotalambda/SurrogateAttribute.Fody/main/icon.png)


A Fody add-in that allows creating C# attributes composed of other attributes, thus making C#'s attributes a bit more useful.

## Installation

Install [Fody](https://github.com/Fody/Fody) and SurrogateAttribute.Fody to each project in which you want to _use_ surrogate attributes:
```powershell
Install-Package Fody
Install-Package SurrogateAttribute.Fody
```

And make sure both dependencies have `PrivateAssets="All"` like so, because they are only needed during build:
```xml
<PackageReference Include="Fody" Version="???" PrivateAssets="All" />
<PackageReference Include="SurrogateAttribute.Fody" Version="???" PrivateAssets="All" />
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

## In a nutshell

Instead of having:

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

you can create attributes that implement `ISurrogateAttribute` and have reusable abstractions suitable to your requirements, e.g.

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

so `Cat` can be simplified:

```c#
class Cat
{
    [RequiredWithLength(MinLength = 3)]
    [Translation(nameof(Translations.CatName))]
    public string Name { get; set; }
}
```

Attributes implementing `ISurrogateAttribute` are replaced with their `TargetAttributes` and then removed from the assembly at build time using IL weaving.

## Supported features
TODO