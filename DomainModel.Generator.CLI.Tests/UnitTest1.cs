using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace DomainModel.Generator.CLI.Tests;

public class ModelReflectorTests
{
    [Fact]
    public void PublicClass_ShouldHaveOneNode()
    {
        var sut = CreateSut();
        var graph = sut.ReflectTypes(types: new[] { typeof(TestClass1) });
        graph.Nodes.Length.Should().Be(1);
        graph.Nodes[0].Name.Should().Be("TestClass1");
    }

    [Fact]
    public void AnonymousType_ShouldBeSkipped()
    {
        var expected = new { A = "A", B = 3 };
        var sut = CreateSut();
        var graph = sut.ReflectTypes(types: new[] { expected.GetType() });
        graph.Nodes.Length.Should().Be(0);
    }

    [Fact]
    public void InternalClass_ShouldBeSkipped()
    {
        var sut = CreateSut();
        var graph = sut.ReflectTypes(types: new[] { typeof(InternalClass1) });
        graph.Nodes.Length.Should().Be(0);
    }

    [Fact]
    public void NestedClass_ShouldBeSkipped()
    {
        var sut = CreateSut();
        var graph = sut.ReflectTypes(types: new[] { typeof(NestingClass1), typeof(NestingClass1.NestedClass1) });
        graph.Nodes.Length.Should().Be(1);
        graph.Nodes[0].Name.Should().Be("NestingClass1");
    }

    [Fact]
    public void PublicEnum_ShouldHaveOneNode()
    {
        var sut = CreateSut();
        var graph = sut.ReflectTypes(types: new[] { typeof(PublicEnum) });
        graph.Nodes.Length.Should().Be(1);
    }

    [Fact]
    public void TwoClasses_WithReference_ShouldHaveRelation()
    {
        var sut = CreateSut();
        var graph = sut.ReflectTypes(new[] { typeof(TestClass1), typeof(TestClass2) });
        graph.Nodes.Length.Should().Be(2);
        graph.TryGetNodeFor(typeof(TestClass1), out var node1).Should().BeTrue();
        graph.TryGetNodeFor(typeof(TestClass2), out var node2).Should().BeTrue();
        graph.Edges.Length.Should().Be(1);
        var edge = graph.Edges[0];
        edge.From.Should().Be(node2);
        edge.To.Should().Be(node1);
    }

    [Fact]
    public void TwoClasses_WithReferenceButInDifferentOrder_ShouldHaveRelation()
    {
        var sut = CreateSut();
        var graph = sut.ReflectTypes(new[] { typeof(TestClass2), typeof(TestClass1) });
        graph.Nodes.Length.Should().Be(2);
        graph.TryGetNodeFor(typeof(TestClass1), out var node1).Should().BeTrue();
        graph.TryGetNodeFor(typeof(TestClass2), out var node2).Should().BeTrue();
        graph.Edges.Length.Should().Be(1);
        var edge = graph.Edges[0];
        edge.From.Should().Be(node2);
        edge.To.Should().Be(node1);
    }

    [Theory]
    [InlineData(null, null, true)]
    [InlineData(null, "xxx.yyy", true)]
    [InlineData(new[] { "xxx" }, "xxx.yyy", true)]
    [InlineData(new[] { "xxx", "yyy" }, "xxx.yyy", true)]
    [InlineData(new[] { "xxx", "zzz" }, "xxx.yyy", true)]
    [InlineData(new[] { "xxx", "zzz" }, "Xxx.Yyy", false)]
    [InlineData(new[] { "xxx" }, "xxxyyy", false)]
    [InlineData(new[] { "yyy", "zzz" }, "xxx.yyy", false)]
    [InlineData(new[] { "xxx" }, "xxyy", false)]
    [InlineData(new[] { "xxx" }, null, false)]
    public void Options_WithNamesapces(string[] namespaces, string actualNamespace, bool expectedResult)
    {
        var o = new Options(modulePath: "x", generateOptions: null, namespaces: namespaces);
        o.ShouldBe(actualNamespace).Should().Be(expectedResult);
    }

    [Fact]
    public void TestFunction()
    {
        var node = new Node(typeof(TestClass3));
        node.AddPublicAttribute("generic", typeof(List<int>));
        node.AddPublicAttribute("nullable", typeof(int?));
        node["generic"].Should().Be("List<int>");

        node["nullable"].Should().Be("int?");
    }

    private ModelReflector CreateSut()
    {
        return new ModelReflector(options: new Options(
              modulePath: "module.dll",
              generateOptions: new GenerateOptions(
                outputPath: "example.mmd",
                diagramType: "class",
                outputFormat: "??")
            ));
    }
}

class InternalClass1
{
    public int MyProperty { get; set; }
}

public class TestClass1
{
    public int MyProperty { get; set; }
}

public class TestClass2
{
    public TestClass1 TestClass1 { get; set; } = new();
}

public class NestingClass1
{
    public class NestedClass1
    {

    }
    public NestedClass1 NestedProperty { get; set; }
}

public class TestClass3
{
    public List<int> Count { get; set; }

}
public enum PublicEnum
{
    One,
    Two,
    Three
}
