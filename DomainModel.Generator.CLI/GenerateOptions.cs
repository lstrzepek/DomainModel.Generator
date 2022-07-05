namespace DomainModel.Generator.CLI;
public class Options
{
    public Options(GenerateOptions generateOptions,
                   string modulePath,
                   params string[] namespaces)
    {
        ModulePath = modulePath;
        GenerateOptions = generateOptions;
        Namespaces = namespaces;
    }

    public string ModulePath { get; }
    public GenerateOptions GenerateOptions { get; }
    public string[]? Namespaces { get; }

    public bool ShouldBe(string? @namespace) =>
            Namespaces is null ||
            Namespaces.Length == 0 ||
            Namespaces.Any(ns => @namespace is not null && (@namespace == ns || @namespace.StartsWith(ns + ".")));
}
public class GenerateOptions
{
    public string OutputPath { get; }
    public string DiagramType { get; }
    public string OutputFormat { get; }

    public GenerateOptions(
                           string diagramType,
                           string outputPath,
                           string outputFormat)
    {
        OutputPath = outputPath;
        DiagramType = diagramType;
        OutputFormat = outputFormat;
    }
}

