namespace DomainModel.Generator.CLI;
public class Options
{
    public Options(GenerateOptions generateOptions,
                   string modulePath,
                   string[]? includeNamespaces = default,
                   string[]? excludeNamespaces = default,
                   string[]? includeTypes = default,
                   string[]? excludeTypes = default)
    {
        ModulePath = modulePath;
        GenerateOptions = generateOptions;
        IncludeNamespaces = includeNamespaces ?? Array.Empty<string>();
        ExcludeNamespaces = excludeNamespaces ?? Array.Empty<string>();
        IncludeTypes = includeTypes ?? Array.Empty<string>();
        ExcludeTypes = excludeTypes ?? Array.Empty<string>();
    }

    public string ModulePath { get; }
    public GenerateOptions GenerateOptions { get; }
    public string[] IncludeNamespaces { get; }
    public string[] ExcludeNamespaces { get; }
    public string[] IncludeTypes { get; }
    public string[] ExcludeTypes { get; }

    public bool ShouldReflect(Type type)
    {
        if (!type.IsPublic || type.IsAutoClass)
            return false;

        if (ExcludeTypes.Length > 0 && ExcludeTypes.Any(t => t == string.Format("{0}.{1}", type.Namespace, type.Name)))
            return false;

        if (IncludeTypes.Length > 0 && IncludeTypes.Any(t => t == string.Format("{0}.{1}", type.Namespace, type.Name)))
            return true;

        if (ExcludeNamespaces.Length > 0 && ExcludeNamespaces.Any(ns => type.Namespace is not null && (type.Namespace == ns || type.Namespace.StartsWith(ns + "."))))
            return false;

        if ((IncludeNamespaces.Length == 0 && IncludeTypes.Length == 0) || IncludeNamespaces.Any(ns => type.Namespace is not null && (type.Namespace == ns || type.Namespace.StartsWith(ns + "."))))
            return true;

        return false;
    }
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

