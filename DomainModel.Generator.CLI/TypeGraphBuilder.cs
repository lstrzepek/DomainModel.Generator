
public class TypeGraphBuilder
{
    private readonly Graph graph;
    private readonly INodeConnectionStrategy[] conn;

    public TypeGraphBuilder(INodeConnectionStrategy[] conn)
    {
        this.graph = new Graph();
        this.conn = conn;
    }

    public Node AddNode(Node node)
    {
        var newNode = graph.AddNode(node);
        ConnectNodes(newNode);
        return newNode;
    }

    public Graph Build() => this.graph;

    private void ConnectNodes(Node newNode)
    {
        foreach (var node in graph.Nodes)
        {
            if (newNode == node)
                continue;

            if (conn.Any(c => c.AreConnected(node, newNode)))
            {
                graph.AddEdge(node, newNode);
            }

            if (conn.Any(c => c.AreConnected(newNode, node)))
            {
                graph.AddEdge(newNode, node);
            }
        }
    }
}
