public static class TypeExtensions
{
    // https://stackoverflow.com/a/56352691
    // This is the set of types from the C# keyword list.
    static Dictionary<Type, string> _typeAlias = new() {
      { typeof(bool), "bool" },
      { typeof(byte), "byte" },
      { typeof(char), "char" },
      { typeof(decimal), "decimal" },
      { typeof(double), "double" },
      { typeof(float), "float" },
      { typeof(int), "int" },
      { typeof(long), "long" },
      { typeof(object), "object" },
      { typeof(sbyte), "sbyte" },
      { typeof(short), "short" },
      { typeof(string), "string" },
      { typeof(uint), "uint" },
      { typeof(ulong), "ulong" },
      // Yes, this is an odd one.  Technically it's a type though.
      { typeof(void), "void" }
    };

    static private string GetTypeNameOrAlias(Type type)
    {

        // Handle nullable value types
        var nullbase = Nullable.GetUnderlyingType(type);
        if (nullbase != null)
            return GetTypeNameOrAlias(nullbase) + "?";

        // Handle generic types
        if (type.IsGenericType)
        {
            string name = type.Name.Split('`').FirstOrDefault();
            IEnumerable<string> parms =
                type.GetGenericArguments()
                .Select(a => type.IsConstructedGenericType ? GetTypeNameOrAlias(a) : a.Name);
            return $"{name}<{string.Join(",", parms)}>";
        }

        // Handle arrays
        if (type.BaseType == typeof(System.Array))
            return GetTypeNameOrAlias(type.GetElementType()) + "[]";

        // Lookup alias for type
        if (_typeAlias.TryGetValue(type, out string alias))
            return alias;

        // Default to CLR type name
        return type.Name;
    }

    public static string FormatTypeName(this Type type)
    {
        string name = GetTypeNameOrAlias(type);
        if (type.DeclaringType is Type dec)
        {
            return $"{FormatTypeName(dec)}.{name}";
        }
        return name;
    }

    public static Type GetClassType(this Type type)
    {
        var nodeName = type;
        if (type.IsArray)
        {
            nodeName = type.GetElementType();
        }
        else if (type.IsGenericType && type.FullName.StartsWith("System.Collection"))
        {
            nodeName = type.GetGenericArguments()[0];
        }
        return nodeName;

    }
}
