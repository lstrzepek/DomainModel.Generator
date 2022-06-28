
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
        if (TryGetConnectedNode(type, out var otherNode))
        {
            graph.AddEdge(node, otherNode);
        }
    }

    public Graph Build() => this.graph;

    private bool TryGetConnectedNode(Type type, out Node node)
    {
        var nodeName = type.GetClassType();
        return graph.TryGetNodeFor(nodeName, out node);
    }
}
