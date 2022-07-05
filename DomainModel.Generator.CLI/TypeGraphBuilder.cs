
public class TypeGraphBuilder
{
    private readonly Graph graph;

    public TypeGraphBuilder()
    {
        this.graph = new Graph();
    }

    public Node AddType(Type type)
    {
        var newNode = graph.AddNode(type);
        foreach (var node in graph.Nodes)
        {
            if (node.Attributes.Any(a => a.type == type.Name))
            {
                graph.AddEdge(node, newNode);
            }
        }
        return newNode;
    }

    public void AddPublicAttribute(Node node, string name, Type type)
    {
        node.AddPublicAttribute(name, type);
        foreach (var otherNode in TryGetConnectedNodes(type))
        {
            graph.AddEdge(node, otherNode);
        }
    }

    public Graph Build() => this.graph;

    private Node[] TryGetConnectedNodes(Type type)
    {
        var types = type.GetContainingTypes();
        return graph.FindNodes(types);
    }
}
