using SurrogateAttribute;
using System.ComponentModel.DataAnnotations;

namespace SampleApp;

[AttributeUsage(AttributeTargets.Property)]
class TranslationAttribute(string translationKey) : Attribute, ISurrogateAttribute
{
    Attribute[] ISurrogateAttribute.TargetAttributes => [new DisplayAttribute { ResourceType = typeof(Translations), Name = translationKey }];
}