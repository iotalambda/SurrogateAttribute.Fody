using SampleApp;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;


var cat = new Cat { Name = "Sprinkly-winkly-sprinkles" };
var errors = Validate(cat);
Debug.Assert(errors.Length == 2);
Debug.Assert(errors.Contains("The name does not have an appropriate length."));
Debug.Assert(errors.Contains("Animal identifier is a required field."));


cat.Name = "Sprinkles";
cat.Identifier = "SPRINKLES123";
errors = Validate(cat);
Debug.Assert(errors.Length == 0);


var catIntroduction = $"{Translate(typeof(Cat).GetProperty(nameof(Cat.Name)))}: {cat.Name}.";
Debug.Assert(catIntroduction == "This cat has the following name: Sprinkles.");


var dog = new SampleLib2.Dog { TheDogId = "temp value" };
errors = Validate(dog);
Debug.Assert(errors[0] == "Animal identifier can only contain alphanumeric characters, '_' and '-'.");


static string[] Validate(object obj)
{
    var errors = new List<ValidationResult>();
    Validator.TryValidateObject(obj, new(obj), errors, true);
    return errors.Select(r => r.ErrorMessage).ToArray();
}

static string Translate(PropertyInfo propertyInfo) => propertyInfo.GetCustomAttribute<DisplayAttribute>().GetName();