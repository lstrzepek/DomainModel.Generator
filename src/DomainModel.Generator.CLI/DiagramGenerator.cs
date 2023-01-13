using DomainModel.Generator.Mermaid;

namespace DomainModel.Generator.CLI;

public interface IDiagramGenerator
{
    string GenerateDiagram(Graph graph);
}
public class ClassDiagramGenerator : IDiagramGenerator
{
    public string GenerateDiagram(Graph graph)
    {
        var diagramGenerator = new DomainModel.Generator.Mermaid.ClassDiagramBuilder();
        var map = new Dictionary<Node, IClass>();
        foreach (var node in graph.Nodes)
        {
            IClass @class;
            if (node.Type.IsEnum)
            {
                @class = diagramGenerator.AddClass(name: node.Name, annotation: "enumeration");
            }
            else
            {
                @class = diagramGenerator.AddClass(name: node.Name);
            }
            map.Add(node, @class);
            foreach (var attribute in node.Attributes)
            {
                @class.AddPublicAttribute(attribute.name, attribute.type);
            }
        }
        foreach (var edge in graph.Edges)
        {
            diagramGenerator.LinkWithAssociation(map[edge.From], map[edge.To]);
        }
        return diagramGenerator.Build();
    }
}
