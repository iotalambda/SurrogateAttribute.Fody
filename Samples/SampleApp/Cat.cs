namespace SampleApp;

class Cat
{
    [RequiredWithLength(MinLength = 3)]
    [Translation(nameof(Translations.CatName))]
    public string Name { get; set; }
}