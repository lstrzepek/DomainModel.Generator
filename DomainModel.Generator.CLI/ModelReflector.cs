using System;
using System.Reflection;
using System.Collections;
using DomainModel.Generator.Mermaid;
namespace DomainModel.Generator.CLI;

public class Node
{
    public Node(string name)
    {
        Name = name;
    }

    public string Name { get; }
    public void AddPublicAttribute(PropertyInfo property)
    {

    }
}
public class Graph
{
    List<Node> nodes = new();
    public Node[] Nodes => nodes.ToArray();
    public Node AddNode(Type type)
    {
        var node = new Node(type.Name);
        nodes.Add(node);
        return node;
    }
}
public class ModelReflector
{
    private readonly ClassDiagramGenerator classDiagramGenerator;
    private readonly Options _options;

    public ModelReflector(ClassDiagramGenerator classDiagramGenerator)
    {
        this.classDiagramGenerator = classDiagramGenerator;
    }
    public ModelReflector(Options options)
    {
        this._options = options;
    }
    public Graph ReflectTypes(Type[] types)
    {
        var graph = new Graph();
        foreach (var type in types)
        {
            if (!_options.ShouldBe(type.Namespace))
                continue;

            var node = graph.AddNode(type);
            try
            {
                foreach (var property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    node.AddPublicAttribute(property);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Error when reflecting ${type.Name}: " + ex.Message);
            }

        }
        return graph;
    }
    public string Reflect(Type[] types, Options options)
    {
        foreach (var type in types)
        {
            if (!options.ShouldBe(type.Namespace))
                continue;

            var @class = classDiagramGenerator.AddClass(type.Name);

            try
            {
                foreach (var property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    @class.AddPublicAttribute(property.Name, FormatTypeName(property.PropertyType));
                    TryCreateRelation(options, @class, property.PropertyType);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Cannot locate: " + ex.Message);

            }
        }

        return classDiagramGenerator.Generate();
    }

    private void TryCreateRelation(Options options, IClass @class, Type propertyType)
    {
        var relationType = GetClassName(propertyType);
        if (relationType.IsClass && relationType.Name != "String" && options.ShouldBe(relationType.Namespace))
        {
            var relation = classDiagramGenerator.FindClass(relationType.Name);
            if (relation is null)
                relation = classDiagramGenerator.AddClass(relationType.Name);
            classDiagramGenerator.LinkWithAssociation(@class, relation);
        }
    }

    private string FormatTypeName(Type propertyType)
    {
        try
        {
            if (propertyType.IsGenericType)
            {
                if (propertyType.FullName.StartsWith("System.Collections"))
                    return $"{propertyType.GenericTypeArguments[0].Name}[]";

                return $"{propertyType.Name.Substring(0, propertyType.Name.IndexOf('`'))}~{propertyType.GenericTypeArguments[0].Name}~";
            }
            return propertyType.Name;
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine("Cannot locate: " + ex.Message);
            return "~Unknown~";
        }
    }

    private Type GetClassName(Type type)
    {
        if (type.IsGenericType)
        {
            return type.GenericTypeArguments[0];
        }
        if (type.IsArray)
        {
            return type.GetElementType();
        }
        return type;
    }
}
