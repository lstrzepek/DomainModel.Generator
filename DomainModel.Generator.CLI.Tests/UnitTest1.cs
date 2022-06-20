using FluentAssertions;
using Xunit;

namespace DomainModel.Generator.CLI.Tests;

public class ModelReflectorTests
{
    [Fact]
    public void OneClass_ShouldHaveOneNode()
    {
        var sut = CreateSut();
        var graph = sut.ReflectTypes(types: new[] { typeof(TestClass1) });
        graph.Nodes.Length.Should().Be(1);
        graph.Nodes[0].Name.Should().Be("TestClass1");
    }

    [Fact]
    public void TwoClasses_WithReference_ShouldHaveRelation()
    {
        var sut = CreateSut();
        var graph = sut.ReflectTypes(new[] { typeof(TestClass1), typeof(TestClass2) });
        graph.Nodes.Length.Should().Be(2);
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

class TestClass1
{
    public int MyProperty { get; set; }
}

class TestClass2
{
    public TestClass1 TestClass1 { get; set; } = new();
}
