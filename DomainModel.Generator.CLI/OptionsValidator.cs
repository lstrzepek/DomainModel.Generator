namespace DomainModel.Generator.CLI;

public class OptionsValidator
{
    public void AssertOptions(Options options)
    {
        var modulePath = Path.Combine(Directory.GetCurrentDirectory(), options.ModulePath);
        if (!File.Exists(modulePath))
            throw new ArgumentException($"Cannot find file: {modulePath}");
    }
}