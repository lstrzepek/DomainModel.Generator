namespace DomainModel.Generator.CLI.Tests;

public class ArgsParserTest
{
    [Theory]
    [InlineData(new[] { "generate", "-m", "./path/to/domain.dll", "-o", "./path/to/output.mmd" }, 0, "./path/to/domain.dll", "./path/to/output.mmd", "class", "mermaid")]
    [InlineData(new[] { "generate", "-o", "./path/to/output.mmd", "-m", "./path/to/domain.dll" }, 0, "./path/to/domain.dll", "./path/to/output.mmd", "class", "mermaid")]
    [InlineData(new[] { "generate", "-m", "./path/to/domain.dll", "-o", "./path/to/output.mmd", "--diagram", "class", "--format", "mermaid" }, 0, "./path/to/domain.dll", "./path/to/output.mmd", "class", "mermaid")]
    [InlineData(new[] { "generate", "-m", "./path/to/domain.dll", "-o", "./path/to/output.mmd", "--diagram", "yyy", "--format", "xxx" }, 0, "./path/to/domain.dll", "./path/to/output.mmd", "yyy", "xxx")]
    [InlineData(new[] { "generate", "-m", "./path/to/domain.dll", "-o", "./path/to/output.mmd", "-n", "namespace1", "-n", "namespace2" }, 0, "./path/to/domain.dll", "./path/to/output.mmd", "class", "mermaid")]
    [InlineData(new[] { "generate", "--module", "./path/to/domain.dll", "--output", "./path/to/output.mmd" }, 0, "./path/to/domain.dll", "./path/to/output.mmd", "class", "mermaid")]
    [InlineData(new[] { "generate", "--output", "./path/to/output.mmd", "--module", "./path/to/domain.dll" }, 0, "./path/to/domain.dll", "./path/to/output.mmd", "class", "mermaid")]
    [InlineData(new[] { "generate", "--module", "./path/to/domain.dll", "--output", "./path/to/output.mmd", "--diagram", "class", "--format", "mermaid" }, 0, "./path/to/domain.dll", "./path/to/output.mmd", "class", "mermaid")]
    [InlineData(new[] { "generate", "--module", "./path/to/domain.dll", "--output", "./path/to/output.mmd", "--diagram", "yyy", "--format", "xxx" }, 0, "./path/to/domain.dll", "./path/to/output.mmd", "yyy", "xxx")]
    [InlineData(new[] { "generate", "--module", "./path/to/domain.dll", "--output", "./path/to/output.mmd", "--namespace", "namespace1", "--namespace", "namespace2"  }, 0, "./path/to/domain.dll", "./path/to/output.mmd", "class", "mermaid")]
    public void ValidArgs_ShouldPass(string[] args,
                                     int expectedResult,
                                     string expectedModulePath,
                                     string expectedOutputPath,
                                     string expectedDiagramType,
                                     string expectedOutputFormat)
    {
        var result = ArgsParser.Run(args, (options) =>
            {
                options.ModulePath.Should().Be(expectedModulePath);
                options.GenerateOptions.DiagramType.Should().Be(expectedDiagramType);
                options.GenerateOptions.OutputFormat.Should().Be(expectedOutputFormat);
                options.GenerateOptions.OutputPath.Should().Be(expectedOutputPath);
                return 0;
            });

        result.Should().Be(expectedResult, "when this failed it means backward compatibility is broken");
    }
}