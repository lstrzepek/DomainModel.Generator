using System.Linq;
using System.Collections.Generic;
namespace DomainModel.Generator.Mermaid;

public interface IClass
{
    string Name { get; }
    void AddPublicAttribute(string name, string? type = default);
}

public class ClassDiagramGenerator
{
    private readonly System.Text.StringBuilder diagramBuilder;
    private readonly Dictionary<string, ClassDescription> classes;
    private readonly Dictionary<HashSet<string>, Relation> relations;


    public ClassDiagramGenerator()
    {
        diagramBuilder = new("classDiagram");
        classes = new();
        relations = new(HashSet<string>.CreateSetComparer());
    }

    public IClass AddClass(string name)
    {
        if (!classes.ContainsKey(name))
            classes.Add(name, new ClassDescription(name));

        return classes[name];
    }

    public IClass? FindClass(string name)
    {
        classes.TryGetValue(name, out var @class);
        return @class;
    }
    private void AddRelation(IClass classA, IClass classB, string symbol, string? label = default)
    {
        HashSet<string> key = new (new[] { classA.Name, classB.Name });
        if (!relations.ContainsKey(key))
            relations.Add(key, new Relation(
                classA.Name,
                classB.Name,
                symbol,
                label
            ));
    }
    public void LinkWithInheritance(IClass classA, IClass classB, string? label = default)
        => AddRelation(classA, classB, "<|--", label);
    public void LinkWithComposition(IClass classA, IClass classB, string? label = default)
        => AddRelation(classA, classB, "*--", label);
    public void LinkWithAggregation(IClass classA, IClass classB, string? label = default)
        => AddRelation(classA, classB, "o--", label);
    public void LinkWithAssociation(IClass classA, IClass classB, string? label = default)
        => AddRelation(classA, classB, "<--", label);
    public void LinkSolid(IClass classA, IClass classB, string? label = default)
        => AddRelation(classA, classB, "--", label);
    public void LinkDashed(IClass classA, IClass classB, string? label = default)
        => AddRelation(classA, classB, "..", label);
    public void LinkWithDependency(IClass classA, IClass classB, string? label = default)
        => AddRelation(classA, classB, "<..", label);
    public void LinkWithRealization(IClass classA, IClass classB, string? label = default)
        => AddRelation(classA, classB, "<|..", label);

    private bool RelationExist(string className) => relations.Keys.Any(r => r.Contains(className));

    public string Generate()
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
        public ClassDescription(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new System.ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
            }
            Name = name;
            diagramBuilder = new($"\tclass {name}" + Environment.NewLine);
        }
        public string Name { get; }

        private void AddAttribute(string visibility, string name, string? type)
        {
            IsEmpty = false;
            diagramBuilder.AppendLine(string.IsNullOrEmpty(type) ?
            $"\t{Name} : {visibility}{name}" :
            $"\t{Name} : {visibility}{type} {name}");
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

