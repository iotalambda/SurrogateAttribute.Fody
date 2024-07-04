using SampleApp;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;


var cat = new Cat { Name = "Sprinkly-winkly-sprinkles" };
var errors = Validate(cat);
Debug.Assert(errors.Single() == "The name does not have an appropriate length.");


cat.Name = "Sprinkles";
errors = Validate(cat);
Debug.Assert(errors.Length == 0);


var catIntroduction = $"{Translate(typeof(Cat).GetProperty(nameof(Cat.Name)))}: {cat.Name}.";
Debug.Assert(catIntroduction == "This cat has the following name: Sprinkles.");


static string[] Validate(object obj)
{
    var errors = new List<ValidationResult>();
    Validator.TryValidateObject(obj, new(obj), errors, true);
    return errors.Select(r => r.ErrorMessage).ToArray();
}

static string Translate(PropertyInfo propertyInfo) => propertyInfo.GetCustomAttribute<DisplayAttribute>().GetName();