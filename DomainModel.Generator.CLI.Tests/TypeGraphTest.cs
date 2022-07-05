using System.Collections.Generic;

namespace DomainModel.Generator.CLI.Tests;
public class TypeGraphTest
{
    [Fact]
    public void TestFunction()
    {
        var node = new Node(typeof(string));
        node.AddPublicAttribute("generic", typeof(List<int>));
        node.AddPublicAttribute("openGeneric", typeof(IDictionary<,>));
        node.AddPublicAttribute("nullable", typeof(int?));
        node["generic"].Should().Be("List<int>");
        node["openGeneric"].Should().Be("IDictionary<TKey,TValue>");
        node["nullable"].Should().Be("int?");
    }

}
