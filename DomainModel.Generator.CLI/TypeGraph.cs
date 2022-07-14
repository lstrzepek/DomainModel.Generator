public class Node
{
    private readonly Dictionary<string, string> attributes = new();

    public Node(Type type)
    {
        Type = type;
    }
    public string this[string name] => attributes[name];
    public string Name => Type.Name;
    public (string name, string type)[] Attributes => attributes.Select(a => (a.Key, a.Value)).ToArray();

    public Type Type { get; }

    public void AddPublicAttribute(string name, Type type)
    {
        if (attributes.ContainsKey(name))
        {
            attributes[name] = type.FormatTypeName();
            return;
        }
        attributes.Add(name, type.FormatTypeName());
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
        if (type.FullName is null)
            throw new ArgumentNullException(nameof(type.FullName));

        if (nodes.ContainsKey(type.FullName))
            return nodes[type.FullName];

        var node = new Node(type);
        nodes.Add(type.FullName, node);
        return node;
    }

    public bool TryGetNodeFor(Type type, out Node? node)
    {
        var typeFullName = type.FullName;
        if (string.IsNullOrWhiteSpace(typeFullName))
        {
            node = null;
            return false;
        }
        return nodes.TryGetValue(typeFullName, out node);
    }

    public Node[] FindNodes(params Type[] types)
    {
        return types.Where(t => t.FullName is not null && nodes.ContainsKey(t.FullName))
                    .Select(t => nodes[t.FullName!]).ToArray();
    }

    internal Edge AddEdge(Node from, Node to)
    {
        var edge = new Edge(from, to);
        edges.Add(edge);
        return edge;
    }
}
