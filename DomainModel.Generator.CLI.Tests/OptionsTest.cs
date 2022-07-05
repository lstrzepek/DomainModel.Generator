namespace DomainModel.Generator.CLI.Tests;

public class OptionsTest
{

    [Theory]
    [InlineData(null, null, true)]
    [InlineData(null, "xxx.yyy", true)]
    [InlineData(new[] { "xxx" }, "xxx", true)]
    [InlineData(new[] { "xxx" }, "xxx.yyy", true)]
    [InlineData(new[] { "xxx", "yyy" }, "xxx.yyy", true)]
    [InlineData(new[] { "xxx", "zzz" }, "xxx.yyy", true)]
    [InlineData(new[] { "xxx", "zzz" }, "Xxx.Yyy", false)]
    [InlineData(new[] { "xxx" }, "xxxyyy", false)]
    [InlineData(new[] { "yyy", "zzz" }, "xxx.yyy", false)]
    [InlineData(new[] { "xxx" }, "xxyy", false)]
    [InlineData(new[] { "xxx" }, null, false)]
    public void Options_WithNamespaces(string[] namespaces, string actualNamespace, bool expectedResult)
    {
        var o = new Options(modulePath: "x", generateOptions: null, namespaces: namespaces);
        o.ShouldBe(actualNamespace).Should().Be(expectedResult);
    }
}
