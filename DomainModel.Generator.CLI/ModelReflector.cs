using System;
using System.Reflection;
using System.Collections;
using DomainModel.Generator.Mermaid;
namespace DomainModel.Generator.CLI;

public class ModelReflector
{
    private readonly ClassDiagramGenerator classDiagramGenerator;

    public ModelReflector(ClassDiagramGenerator classDiagramGenerator)
    {
        this.classDiagramGenerator = classDiagramGenerator;
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