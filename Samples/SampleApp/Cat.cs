using SampleLib;

namespace SampleApp;

class Cat
{
    [AnimalIdentifier]
    public string Identifier { get; set; }

    [RequiredWithLength(MinLength = 3)]
    [Translation(nameof(Translations.CatName))]
    public string Name { get; set; }
}