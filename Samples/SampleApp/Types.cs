using SurrogateAttribute;
using System.ComponentModel.DataAnnotations;

namespace SampleApp;

class Cat
{
    [RequiredWithLength(MinLength = 3)]
    [Translation(nameof(Translations.CatName))]
    public string Name { get; set; }
}

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
    Attribute[] ISurrogateAttribute.TargetAttributes => [new DisplayAttribute { ResourceType = typeof(Translations), Name = translationKey }];
}