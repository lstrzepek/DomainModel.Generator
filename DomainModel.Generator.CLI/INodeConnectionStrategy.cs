public interface INodeConnectionStrategy
{
    bool AreConnected(Node from, Node to);
}

/**
<summary>Match by direct reference of type</summary>
<example>
class A {}
class B {
    public A RefToA {get; set;}
}
</example>
*/
public class ByTypeNodeConnectionStrategy : INodeConnectionStrategy
{
    public bool AreConnected(Node from, Node to)
    {
        //TODO: Solve potential problem with formatted attribute name vs type name
        var isConnected = (Node a, Node b) => a.Attributes.Any(n => n.type == b.Name);
        return isConnected(from, to);
    }
}

/**
<summary>Match by indirect reference by id</summary>
<example>
class A {}
class B {
    public Guid AId {get; set;}
    OR
    public Guid[] AIds {get; set;}
}
</example>
*/
public class ByIdNodeConnectionStrategy : INodeConnectionStrategy
{
    public bool AreConnected(Node from, Node to)
    {
        //TODO: Solve potential problem with formatted attribute name vs type name
        var isConnected = (Node a, Node b) => a.Attributes.Any(n => (n.name == $"{b.Name}Id") || (n.name == $"{b.Name}Ids"));
        return isConnected(from, to);
    }
}