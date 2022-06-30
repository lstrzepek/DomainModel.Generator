using System.Runtime.InteropServices;
using System.Reflection;

namespace DomainModel.Generator.CLI;
public class ModelLoader
{
    private readonly ModelReflector modelReflector;

    public ModelLoader(ModelReflector modelReflector)
    {
        this.modelReflector = modelReflector;
    }
    public Graph LoadModule(Options options)
    {
        var runtimeDirectory = RuntimeEnvironment.GetRuntimeDirectory();
        Console.WriteLine($"Loading core assemblies from {runtimeDirectory}");
        string[] runtimeAssemblies = Directory.GetFiles(runtimeDirectory, "*.dll");

        var moduleAssemblyPath = Path.GetFullPath(options.ModulePath);
        string[] modelAssemblies = Directory.GetFiles(Path.GetDirectoryName(moduleAssemblyPath)!, "*.dll");
        Console.WriteLine($"Following libraries will be scanned: \n{string.Join(Environment.NewLine, modelAssemblies)}");

        var paths = runtimeAssemblies.Union(modelAssemblies);
        var resolver = new PathAssemblyResolver(paths);
        using var mlc = new MetadataLoadContext(resolver);

        Assembly startupAssembly = mlc.LoadFromAssemblyPath(moduleAssemblyPath);
        var types = startupAssembly.GetTypes();

        return modelReflector.ReflectTypes(types);
    }

}
