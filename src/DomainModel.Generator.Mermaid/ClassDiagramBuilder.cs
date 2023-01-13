namespace DomainModel.Generator.Mermaid;

public interface IClass
{
    string Name { get; }
    void AddPublicAttribute(string name, string? type = default);
}

public class ClassDiagramBuilder
{
    private readonly System.Text.StringBuilder diagramBuilder;
    private readonly Dictionary<string, ClassDescription> classes;
    private readonly Dictionary<HashSet<string>, Relation> relations;


    public ClassDiagramBuilder()
    {
        diagramBuilder = new("classDiagram");
        classes = new();
        relations = new(HashSet<string>.CreateSetComparer());
    }

    public IClass AddClass(string name, string? annotation = default)
    {
        if (!classes.ContainsKey(name))
            classes.Add(name, new ClassDescription(name, annotation));

        return classes[name];
    }

    public IClass? FindClass(string name)
    {
        classes.TryGetValue(name, out var @class);
        return @class;
    }
    private void AddRelation(IClass from, IClass to, string symbol, string? label = default)
    {
        HashSet<string> key = new(new[] { from.Name, to.Name });
        if (!relations.ContainsKey(key))
            relations.Add(key, new Relation(
                from.Name,
                to.Name,
                symbol,
                label
            ));
    }
    public void LinkWithInheritance(IClass from, IClass to, string? label = default)
        => AddRelation(from, to, "--|>", label);
    public void LinkWithComposition(IClass from, IClass to, string? label = default)
        => AddRelation(from, to, "--*", label);
    public void LinkWithAggregation(IClass from, IClass to, string? label = default)
        => AddRelation(from, to, "--o", label);
    public void LinkWithAssociation(IClass from, IClass to, string? label = default)
        => AddRelation(from, to, "-->", label);
    public void LinkSolid(IClass from, IClass to, string? label = default)
        => AddRelation(from, to, "--", label);
    public void LinkDashed(IClass from, IClass to, string? label = default)
        => AddRelation(from, to, "..", label);
    public void LinkWithDependency(IClass from, IClass to, string? label = default)
        => AddRelation(from, to, "..>", label);
    public void LinkWithRealization(IClass from, IClass to, string? label = default)
        => AddRelation(from, to, "..|>", label);

    private bool RelationExist(string className) => relations.Keys.Any(r => r.Contains(className));

    public string Build()
    {
        diagramBuilder.Append(Environment.NewLine);
        foreach (var @class in classes.Values)
        {
            if (@class.IsEmpty && RelationExist(@class.Name))
                continue;
            diagramBuilder.AppendLine(@class.ToString());
        }
        foreach (var relationship in relations.Values)
        {
            diagramBuilder.AppendLine(relationship.ToString());
        }
        return diagramBuilder.ToString().Trim();
    }
    class ClassDescription : IClass
    {
        public bool IsEmpty { get; private set; } = true;
        private readonly System.Text.StringBuilder diagramBuilder;
        public ClassDescription(string name, string? annotation)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new System.ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }
            Name = name;
            diagramBuilder = new($"\tclass {name}" + Environment.NewLine);
            if (!string.IsNullOrWhiteSpace(annotation))
                diagramBuilder.Append($"\t<<{annotation}>> {name}" + Environment.NewLine);
        }
        public string Name { get; }

        private void AddAttribute(string visibility, string name, string? type)
        {
            IsEmpty = false;
            diagramBuilder.AppendLine(string.IsNullOrEmpty(type) ?
            $"\t{Name} : {visibility}{name}" :
            $"\t{Name} : {visibility}{type.Replace('<', '~').Replace('>', '~')} {name}");
        }
        public void AddPublicAttribute(string name, string? type)
            => AddAttribute("+", name, type);
        public void AddPrivateAttribute(string name, string? type)
            => AddAttribute("-", name, type);
        public void AddProtectedAttribute(string name, string? type)
            => AddAttribute("#", name, type);
        public void AddInternalAttribute(string name, string? type)
            => AddAttribute("~", name, type);

        public override string ToString()
        {
            return diagramBuilder.ToString();
        }
    }
}

record Relation(string ClassA, string ClassB, string Symbol, string? Label)
{
    public bool Contains(string className) => ClassA == className || ClassB == className;
    public string GetOtherThan(string className)
    {
        if (className != ClassA && className != ClassB)
            throw new ArgumentException($"{className} is not a part of this relation.");
        return className == ClassA ? ClassB : ClassA;
    }
    public override string ToString() => string.IsNullOrEmpty(Label) ?
    $"{ClassA} {Symbol} {ClassB}" :
    $"{ClassA} {Symbol} {ClassB} : {Label}";

}

