using System;

namespace DomainModel.Generator.CLI.Tests;

public class OptionsTest
{

    [Theory]
    [InlineData(typeof(TestModel.Program), new[] { "TestModel" }, true)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), null, true)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "TestModel" }, true)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "TestModel", "NotExisting" }, true)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "Domain", "NotExisting" }, false)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "testModel", "NotExisting" }, false)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "Test" }, false)]
    public void Options_WithNamespacesIncluded(Type actualType, string[] includeNamespaces, bool expectedResult)
    {
        var sut = CreateSut(includeNamespaces);
        sut.ShouldReflect(actualType).Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), null, true)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "TestModel" }, false)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "TestModel", "NotExisting" }, false)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "Domain", "NotExisting" }, true)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "testModel", "NotExisting" }, true)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "Test" }, true)]
    public void Options_WithNamespacesExcluded(Type actualType, string[] excludeNamespaces, bool expectedResult)
    {
        var sut = CreateSut(excludeNamespaces: excludeNamespaces);
        sut.ShouldReflect(actualType).Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), null, true)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "TestModel.Domain.Customers.Customer" }, true)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "Customer", "NotExisting" }, false)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "TestModel.Domain", "NotExisting" }, false)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "TestModel.Domain.Customers.customer", "NotExisting" }, false)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "TestModel.Domain.Customers.Cu" }, false)]
    public void Options_WithTypesIncluded(Type actualType, string[] includeTypes, bool expectedResult)
    {
        var sut = CreateSut(includeTypes: includeTypes);
        sut.ShouldReflect(actualType).Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), null, true)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "TestModel.Domain.Customers.Customer" }, false)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "TestModel.Domain.Customers.Customer.Address" }, true)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "Customer", "NotExisting" }, true)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "TestModel.Domain", "NotExisting" }, true)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "TestModel.Domain.Customers.customer", "NotExisting" }, true)]
    [InlineData(typeof(TestModel.Domain.Customers.Customer), new[] { "TestModel.Domain.Customers.Cu" }, true)]
    public void Options_WithTypesExcluded(Type actualType, string[] excludeTypes, bool expectedResult)
    {
        var sut = CreateSut(excludeTypes: excludeTypes);
        sut.ShouldReflect(actualType).Should().Be(expectedResult);
    }

    [Fact]
    public void WhenSameValuesInIncludeAndExcludeTypes_ExcludeHasPriority()
    {
        var sut = CreateSut(excludeTypes: new[] { "TestModel.Domain.Customers.Customer" }, includeTypes: new[] { "TestModel.Domain.Customers.Customer" });
        sut.ShouldReflect(typeof(TestModel.Domain.Customers.Customer)).Should().Be(false);
    }

    [Fact]
    public void WhenSameValuesInIncludeAndExcludeNamespaces_ExcludeHasPriority()
    {
        var sut = CreateSut(excludeNamespaces: new[] { "TestModel.Domain.Customers" }, includeNamespaces: new[] { "TestModel.Domain.Customers" });
        sut.ShouldReflect(typeof(TestModel.Domain.Customers.Customer)).Should().Be(false);
    }

    [Fact]
    public void WhenExcludeNamespaceIsMoreExact_MoreExactHasPriority()
    {
        var sut = CreateSut(excludeNamespaces: new[] { "TestModel.Domain.Customers" }, includeNamespaces: new[] { "TestModel.Domain" });
        sut.ShouldReflect(typeof(TestModel.Domain.Customers.Customer)).Should().Be(false);
    }

    [Fact]
    public void WhenIncludeNamespaceIsMoreExact_ExcludeHasPriority()
    {
        var sut = CreateSut(excludeNamespaces: new[] { "TestModel.Domain" }, includeNamespaces: new[] { "TestModel.Domain.Customers" });
        sut.ShouldReflect(typeof(TestModel.Domain.Customers.Customer)).Should().Be(false);
    }


    [Fact]
    public void WhenBothExcludeNamespaceAndIncludeTypeMatch_TypeHasPriority()
    {
        var sut = CreateSut(excludeNamespaces: new[] { "TestModel.Domain" }, includeTypes: new[] { "TestModel.Domain.Customers.Customer" });
        sut.ShouldReflect(typeof(TestModel.Domain.Customers.Customer)).Should().Be(true);
    }

    [Fact]
    public void WhenBothIncludeNamespaceAndExcludeTypeMatch_TypeHasPriority()
    {
        var sut = CreateSut(includeNamespaces: new[] { "TestModel.Domain" }, excludeTypes: new[] { "TestModel.Domain.Customers.Customer" });
        sut.ShouldReflect(typeof(TestModel.Domain.Customers.Customer)).Should().Be(false);
    }
    
    [Fact]
    public void WhenExcludeNamespaceMatchButIncludeNamespaceNot_ShouldNotReflect()
    {
        var sut = CreateSut(excludeNamespaces: new[] { "TestModel.Domain" }, includeNamespaces: new[] { "NotExisting" });
        sut.ShouldReflect(typeof(TestModel.Domain.Customers.Customer)).Should().Be(false);
    }

    [Fact]
    public void WhenIncludeNamespaceMatchButExcludeNamespaceNot_ShouldReflect()
    {
        var sut = CreateSut(includeNamespaces: new[] { "TestModel.Domain" }, excludeNamespaces: new[] { "NotExisting" });
        sut.ShouldReflect(typeof(TestModel.Domain.Customers.Customer)).Should().Be(true);
    }

    private static Options CreateSut(string[]? includeNamespaces = default, string[]? excludeNamespaces = default, string[]? includeTypes = default, string[]? excludeTypes = default)
    {
        return new Options(modulePath: "x",
                           generateOptions: new GenerateOptions("x", "y", "ss"),
                           includeNamespaces: includeNamespaces,
                           excludeNamespaces: excludeNamespaces,
                           includeTypes: includeTypes,
                           excludeTypes: excludeTypes);
    }
}
