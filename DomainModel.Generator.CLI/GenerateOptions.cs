namespace DomainModel.Generator.CLI;
public class Options
{
    public Options(GenerateOptions generateOptions,
                   string modulePath,
                   params string[] namespaces)
    {
        ModulePath = modulePath;
        GenerateOptions = generateOptions;
        InludeNamespaces = namespaces;
    }

    public string ModulePath { get; }
    public GenerateOptions GenerateOptions { get; }
    public string[]? InludeNamespaces { get; }
    public string[]? ExcludeNamespaces { get; }
    public string[]? IncludeTypes { get; }
    public string[]? ExcludeTypes { get; }

    public bool ShouldReflect(string? @namespace) =>
            InludeNamespaces is null ||
            InludeNamespaces.Length == 0 ||
            InludeNamespaces.Any(ns => @namespace is not null && (@namespace == ns || @namespace.StartsWith(ns + ".")));
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

