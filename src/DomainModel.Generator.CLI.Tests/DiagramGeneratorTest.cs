using System;
using TestModel.Domain.Customers;

namespace DomainModel.Generator.CLI.Tests;

public class DiagramGeneratorTest
{
    [Fact]
    public void GenerateDiagram_WhenEmptyGraph_ShouldGenerateEmptyClassDiagram()
    {
        var graph = new Graph();
        var sut = new ClassDiagramGenerator();
        var diagram = sut.GenerateDiagram(graph);
        diagram.Should().Be("classDiagram");
    }

    [Fact]
    public void GenerateDiagram_WhenGraphWithClass_ShouldGenerateEmptyClass()
    {
        var graph = new Graph();
        graph.AddNode(typeof(Customer));
        var sut = new ClassDiagramGenerator();
        var diagram = sut.GenerateDiagram(graph);
        diagram.Should().Be(new[]{
            "classDiagram",
            "\tclass Customer"
            }.ToDiagram());
    }

    [Fact]
    public void GenerateDiagram_WhenNodeWithAttributeWithType_ShouldGenerateTypeForField()
    {
        var graph = new Graph();
        var node = graph.AddNode(typeof(Customer));
        node.AddPublicAttribute("Name", typeof(string));
        var sut = new ClassDiagramGenerator();
        var diagram = sut.GenerateDiagram(graph);
        diagram.Should().Be(new[]{
            "classDiagram",
            "\tclass Customer",
            "\tCustomer : +string Name"
            }.ToDiagram());
    }

    [Fact]
    public void GenerateDiagram_WhenNodeTypOfEnum_ShouldAddEnumerationStereotype()
    {
        var graph = new Graph();
        graph.AddNode(typeof(CustomerType));
        var sut = new ClassDiagramGenerator();
        var diagram = sut.GenerateDiagram(graph);
        diagram.Should().Be(new[]{
            "classDiagram",
            "\tclass CustomerType",
            "\t<<enumeration>> CustomerType"
            }.ToDiagram());
    }

    [Fact]
    public void GenerateDiagram_WhenNodeWithAttributeWithoutType_ShouldGenerateTypeForField()
    {
        var graph = new Graph();
        var node = graph.AddNode(typeof(CustomerType));
        node.AddPublicAttribute("Company");
        var sut = new ClassDiagramGenerator();
        var diagram = sut.GenerateDiagram(graph);
        diagram.Should().Be(new[]{
            "classDiagram",
            "\tclass CustomerType",
            "\t<<enumeration>> CustomerType",
            "\tCustomerType : +Company"
            }.ToDiagram());
    }
}

public static class TestExtensions
{
    public static string ToDiagram(this string[] array)
    {
        return string.Join(Environment.NewLine, array);
    }
}