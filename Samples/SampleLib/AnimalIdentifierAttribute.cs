using SurrogateAttribute;
using System.ComponentModel.DataAnnotations;

namespace SampleLib;

[AttributeUsage(AttributeTargets.Property)]
public class AnimalIdentifierAttribute : Attribute, ISurrogateAttribute
{
    public Attribute[] TargetAttributes => [
        new RequiredAttribute { ErrorMessage = "Animal identifier is a required field." },
        new StringLengthAttribute(50) { ErrorMessage = "Animal identifier cannot be longer than 50 characters." },
        new RegularExpressionAttribute(@"^[a-zA-Z0-9_-]+$") { ErrorMessage = "Animal identifier can only contain alphanumeric characters, '_' and '-'." },
    ];
}
