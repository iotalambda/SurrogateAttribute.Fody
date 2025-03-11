using SurrogateAttribute;
using System.ComponentModel.DataAnnotations;

namespace SampleLib;

[AttributeUsage(AttributeTargets.Property)]
public class AnimalSaysAttribute : Attribute, ISurrogateAttribute
{
    [PropertyDefaultValue(["Meow", "Woof"])]
    string[] AllowedValues { get; }

    Attribute[] ISurrogateAttribute.TargetAttributes => [new AllowedValuesAttribute(AllowedValues) { ErrorMessage = "Animal must say either Meow or Woof." }];
}
