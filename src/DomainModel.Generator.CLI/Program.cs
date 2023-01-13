using DomainModel.Generator.CLI;
using DomainModel.Generator.Mermaid;



return ArgsParser.Run(args, (options) =>
{
    //new OptionsValidator().AssertOptions(options);

    var modelLoader = new ModelLoader(
        new ModelReflector(options));
    var graph = modelLoader.LoadModule(options);
    var generator = new ClassDiagramGenerator();
    var diagram = generator.GenerateDiagram(graph);
    File.WriteAllText(options.GenerateOptions.OutputPath, diagram);
    return 0;
},
onError: result => { Console.Error.WriteLine(result); return 1; },
showHelp: result => { Console.WriteLine(result); return 0; },
showVersion: result => { Console.WriteLine(result); return 0; });
