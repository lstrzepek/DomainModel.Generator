namespace DomainModel.Generator.CLI;

public class ModelReflector
{
    private readonly Options options;

    public ModelReflector(Options options)
    {
        this.options = options;
    }
    public Graph ReflectTypes(Type[] types)
    {
        var graphBuilder = new TypeGraphBuilder();
        foreach (var type in types)
        {
            if (!type.IsPublic || type.IsAutoClass)
                continue;

            if (!options.ShouldBe(type.Namespace))
                continue;

            var node = graphBuilder.AddType(type);
            try
            {
                foreach (var property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    graphBuilder.AddPublicAttribute(node, property.Name, property.PropertyType);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Error when reflecting ${type.Name}: " + ex.Message);
            }

        }
        return graphBuilder.Build();
    }
}
