using System;
using System.Collections.Generic;

namespace DomainModel.Generator.CLI.Tests;

public class ModelReflectorTest
{
    [Fact]
    public void PublicClass_ShouldHaveOneNode()
    {
        var sut = CreateSut();
        var graph = sut.ReflectTypes(types: new[] { typeof(PublicClass) });
        graph.Nodes.Length.Should().Be(1);
        graph.Nodes[0].Name.Should().Be("PublicClass");
    }

    [Fact]
    public void AnonymousType_ShouldBeSkipped()
    {
        var reflectedAnonymousType = new { A = "A", B = 3 };
        var sut = CreateSut();
        var graph = sut.ReflectTypes(types: new[] { reflectedAnonymousType.GetType() });
        graph.Nodes.Length.Should().Be(0);
    }

    [Fact]
    public void InternalClass_ShouldBeSkipped()
    {
        var sut = CreateSut();
        var graph = sut.ReflectTypes(types: new[] { typeof(InternalClass) });
        graph.Nodes.Length.Should().Be(0);
    }

    [Fact]
    public void NestedClass_ShouldBeSkipped()
    {
        var sut = CreateSut();
        var graph = sut.ReflectTypes(types: new[] { typeof(NestingClass), typeof(NestingClass.NestedClass) });
        graph.Nodes.Length.Should().Be(1);
        graph.Nodes[0].Name.Should().Be("NestingClass");
    }

    [Fact]
    public void PublicEnum_ShouldHaveOneNode()
    {
        var sut = CreateSut();
        var graph = sut.ReflectTypes(types: new[] { typeof(PublicEnum) });
        graph.Nodes.Length.Should().Be(1);
    }

    [Fact]
    public void DerivedClass_WhenOverridingAttribute_ShouldHaveOneAttribute()
    {
        var sut = CreateSut();
        var graph = sut.ReflectTypes(types: new[] { typeof(DerivedBaseClass) });
        graph.Nodes.Length.Should().Be(1);
        graph.TryGetNodeFor(typeof(DerivedBaseClass), out var node).Should().BeTrue();
        node!.Attributes.Length.Should().Be(1);
    }

    [Fact]
    public void TwoClasses_WithReference_ShouldHaveRelation()
    {
        var sut = CreateSut();
        var graph = sut.ReflectTypes(new[] { typeof(PublicClass), typeof(ReferenceToPublicClass) });
        graph.Nodes.Length.Should().Be(2);
        graph.TryGetNodeFor(typeof(PublicClass), out var node1).Should().BeTrue();
        graph.TryGetNodeFor(typeof(ReferenceToPublicClass), out var node2).Should().BeTrue();
        graph.Edges.Length.Should().Be(1);
        var edge = graph.Edges[0];
        edge.From.Should().Be(node2);
        edge.To.Should().Be(node1);
    }

    [Fact]
    public void TwoClasses_WithReferenceButInDifferentOrder_ShouldHaveRelation()
    {
        var sut = CreateSut();
        var graph = sut.ReflectTypes(new[] { typeof(ReferenceToPublicClass), typeof(PublicClass) });
        graph.Nodes.Length.Should().Be(2);
        graph.TryGetNodeFor(typeof(PublicClass), out var node1).Should().BeTrue();
        graph.TryGetNodeFor(typeof(ReferenceToPublicClass), out var node2).Should().BeTrue();
        graph.Edges.Length.Should().Be(1);
        var edge = graph.Edges[0];
        edge.From.Should().Be(node2);
        edge.To.Should().Be(node1);
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

class InternalClass
{
    public int MyProperty { get; set; }
}

public class PublicClass
{
    public int MyProperty { get; set; }
}

public class ReferenceToPublicClass
{
    public PublicClass TestClass1 { get; set; } = new();
}

public class NestingClass
{
    public class NestedClass
    {

    }
    public NestedClass NestedProperty { get; set; }
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

public class BaseClass
{
    public virtual IReadOnlyCollection<Func<bool, int>> BaseProperty { get; }
}

public class DerivedBaseClass : BaseClass
{
    public new virtual IReadOnlyCollection<Predicate<int>> BaseProperty => this.BaseProperty;
}
