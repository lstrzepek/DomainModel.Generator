using DocoptNet;
using DomainModel.Generator.CLI;

public class ArgsParser
{
    const string Usage =
    @"Domain model it a tool to analyze your model and generate upd to date diagram which can be embedded into your documentation.

Usage:
  model generate [--diagram=<dt>] (-m=<md> | --module=<md>) (-o=<out> | --output=<out>) [--format=<f>] [-n=<ns>... | --namespace=<ns>...] [--ignore-namespace=<ign>...] [-t=<type>... | --type=<type>...] [--ignore-type=<igt>...]
  model -h | --help
  model --version

Options:
  -h --help                         Show this screen.
  --version                         Show version.
  -m=<md> --module=<md>             **Required** Module where to look for types (eg: MyCompany.Domain.dll).
  -n=<ns>... --namespace=<ns>...    Only types in provided namespaces will be visible on diagram as objects.
  -t=<type>... | --type=<type>...   Types to include even if not defined in provided namespaces.
  --ignore-namespace=<ign>...       Namespaces to ignore.
  --ignore-type=<igt>               Type to ignore.
  -o=<out> --output=<out>           **Required** File where to put markdown with diagram (will be overridden!).
  --format=<f>                      Markdown format. Supported values: mermaid, markdown [default: mermaid].
  --diagram=<dt>                    Type of diagram to generate [default: class].


";

    static Options CreateOptionsFrom(IDictionary<string, ArgValue> arguments) => new Options(
            generateOptions: new GenerateOptions(
                outputPath: (string)arguments["--output"],
                diagramType: (string)arguments["--diagram"],
                outputFormat: (string)arguments["--format"]),
            modulePath: (string)arguments["--module"],
            includeNamespaces: ((StringList)arguments["--namespace"]).ToArray());

    static Version? GetVersion()
    {
        return typeof(Options).Assembly.GetName().Version;
    }

    public static int Run(
        string[] args,
        Func<Options, int> onSuccess,
        Func<string, int>? onError = default,
        Func<string, int>? showHelp = default,
        Func<string, int>? showVersion = default)
    {
        onError = onError ?? (_ => 1);
        showHelp = showHelp ?? (_ => 0);
        showVersion = showVersion ?? (_ => 0);
        
        return Docopt.CreateParser(Usage)
                     .WithVersion("Domain model v" + GetVersion())
                     .Parse(args)
                     .Match<int>(result =>
                     {
                         if (!result["generate"].IsTrue)
                             return 1;
                         var options = CreateOptionsFrom(result);
                         //new OptionsValidator().AssertOptions(options);

                         return onSuccess(options);

                     },
                            (Func<IHelpResult, int>)(result => showHelp(result.Help)),
                            (Func<IVersionResult, int>)(result => showVersion(result.Version)),
                            (Func<IInputErrorResult, int>)(result => onError(result.Usage)));
    }

}



