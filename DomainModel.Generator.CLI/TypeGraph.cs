public class Node
{
    private readonly Dictionary<string, (string TypeName, string TypeFullName)> attributes = new();

    public Node(Type type)
    {
        Type = type;
    }
    public string this[string name] => attributes[name].TypeName;
    public string Name => Type.Name;
    public (string name, string type)[] Attributes => attributes.Select(a => (a.Key, a.Value.TypeName)).ToArray();

    public Type Type { get; }

    public void AddPublicAttribute(string name, Type type)
    {
        attributes.Add(name, (TypeName: type.FormatTypeName(), TypeFullName: type.FullName));
    }
}
public class Edge
{
    public Edge(Node from, Node to)
    {
        From = from;
        To = to;
    }

    public Node From { get; }
    public Node To { get; }
}
public class Graph
{
    private readonly Dictionary<string, Node> nodes = new();
    private readonly List<Edge> edges = new();
    public Node[] Nodes => nodes.Values.ToArray();
    public Edge[] Edges => edges.ToArray();
    public Node AddNode(Type type)
    {
        if (nodes.ContainsKey(type.FullName))
            return nodes[type.FullName];

        var node = new Node(type);
        nodes.Add(type.FullName, node);
        return node;
    }

    public bool TryGetNodeFor(Type type, out Node node)
    {
        return nodes.TryGetValue(type.FullName, out node);
    }

    internal Edge AddEdge(Node from, Node to)
    {
        var edge = new Edge(from, to);
        edges.Add(edge);
        return edge;
    }
}
