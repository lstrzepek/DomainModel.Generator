namespace DomainModel.Generator.Mermaid.Tests;

public class ClassDiagramBuilderTests
{
    [Fact]
    public void EmptyDiagram_ShouldBeValid()
    {
        var sut = new ClassDiagramBuilder();
        var diagram = sut.Build();
        diagram.Should().Be("classDiagram");
    }

    [Fact]
    public void AddEmptyClass_WithoutRelation_ShouldDisplayClassDetails()
    {
        var sut = new ClassDiagramBuilder();
        sut.AddClass("Class1");
        var diagram = sut.Build();
        diagram.Should().Be(new[]{
            "classDiagram",
            "\tclass Class1"
        }.ToDiagram());
    }

    [Fact]
    public void AddEmptyClass_WithAnnotation_ShouldDisplayClassDetails()
    {
        var sut = new ClassDiagramBuilder();
        var @class = sut.AddClass(name: "Class1", annotation: "enumeration");
        var diagram = sut.Build();
        diagram.Should().Be(new[]{
            "classDiagram",
            "\tclass Class1",
            "\t<<enumeration>> Class1"
        }.ToDiagram());
    }

    [Fact]
    public void AddClass_WithAttributeWithType_ShouldDisplayClassDetails()
    {
        var sut = new ClassDiagramBuilder();
        var @class = sut.AddClass("BankAccount");
        @class.AddPublicAttribute("owner", "string");
        var diagram = sut.Build();
        diagram.Should().Be(new[]{
            "classDiagram",
            "\tclass BankAccount",
            "\tBankAccount : +string owner"
        }.ToDiagram());
    }

    [Fact]
    public void AddClass_WithAttributeWithTypeWithSpecialCharacters_ShouldDisplayClassDetails()
    {
        var sut = new ClassDiagramBuilder();
        var @class = sut.AddClass("BankAccount");
        @class.AddPublicAttribute("owner", "List<string>");
        var diagram = sut.Build();
        diagram.Should().Be(new[]{
            "classDiagram",
            "\tclass BankAccount",
            "\tBankAccount : +List~string~ owner"
        }.ToDiagram());
    }

    [Fact]
    public void TwoEmptyClasses_WithRelation_ShouldDisplayRelationOnly()
    {
        var sut = new ClassDiagramBuilder();
        var classA = sut.AddClass("ClassA");
        var classB = sut.AddClass("ClassB");
        sut.LinkWithInheritance(classA, classB);
        var diagram = sut.Build();
        diagram.Should().Be(new[]{
            "classDiagram",
            "ClassA --|> ClassB"
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
