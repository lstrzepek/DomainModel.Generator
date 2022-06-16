using System.Linq;
using DocoptNet;
using DomainModel.Generator.CLI;

const string usage =
@"Domain model it a tool to analyze your model and generate upd to date diagram which can be embedded into your documentation.

Usage:
  model generate [--diagram=<dt>] (-m=<md> | --module=<md>) (-o=<out> | --output=<out>) [--format=<f>] [-n=<ns>... | --namespace=<ns>...]
  model -h | --help
  model --version

Options:
  -h --help                       Show this screen.
  --version                       Show version.
  --diagram=<dt>                  Type of diagram to generate [default: class].
  -n=<ns>... --namespace=<ns>...  Only types in provided namespaces will be visible on diagram as objects.
  -m=<md> --module=<md>           Module where to look for types (eg: MyCompany.Domain.dll).
  -o=<out> --output=<out>         File where to put markdown with diagram (will be replaced!).
  --format=<f>                    Markdown format. Supported values: mermaid, markdown [default: mermaid].

";
static int ShowHelp(string help) { Console.WriteLine(help); return 0; }
static int ShowVersion(string version) { Console.WriteLine(version); return 0; }
static int OnError(string usage) { Console.Error.WriteLine(usage); return 1; }
static int Run(IDictionary<string, ArgValue> arguments)
{
    if (arguments["generate"].IsTrue)
    {
        var options = new Options(
            generateOptions: new GenerateOptions(
                outputPath: (string)arguments["--output"],
                diagramType: (string)arguments["--diagram"],
                outputFormat: (string)arguments["--format"]),
            modulePath: (string)arguments["--module"],
            namespaces: ((StringList)arguments["--namespace"]).ToArray());

        var optionValidator = new OptionsValidator();
        optionValidator.AssertOptions(options);

        var modelLoader = new ModelLoader(
            new ModelReflector(
                new DomainModel.Generator.Mermaid.ClassDiagramGenerator()));
        var diagram = modelLoader.LoadModule(options);
        File.WriteAllText(options.GenerateOptions.OutputPath, diagram);
        return 0;
    }
    return 1;
}
return Docopt.CreateParser(usage)
             .WithVersion("Domain model v0.1")
             .Parse(args)
             .Match(Run,
                    result => ShowHelp(result.Help),
                    result => ShowVersion(result.Version),
                    result => OnError(result.Usage));


