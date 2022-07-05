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

    static Type UnknownType = typeof(object);
    static string UnknownTypeName = UnknownType.Name;

    static private string GetTypeNameOrAlias(Type type)
    {
        // Handle nullable value types
        var baseType = Nullable.GetUnderlyingType(type);
        if (baseType != null)
            return GetTypeNameOrAlias(baseType) + "?";

        // Handle generic types
        if (type.IsGenericType)
        {
            string name = type.Name.Split('`').FirstOrDefault(s => !string.IsNullOrWhiteSpace(s)) ?? type.Name;
            var parameters =
                type.GetGenericArguments()
                .Select(a => type.IsConstructedGenericType ? GetTypeNameOrAlias(a) : a.Name);
            return $"{name}<{string.Join(",", parameters)}>";
        }

        // Handle arrays
        if (type.BaseType == typeof(System.Array))
            return GetTypeNameOrAlias(type.GetElementType() ?? UnknownType) + "[]";

        // Lookup alias for type
        if (_typeAlias.TryGetValue(type, out string? alias))
            return alias;

        // Default to CLR type name
        return type.Name;
    }

    public static string FormatTypeName(this Type type)
    {
        string name = GetTypeNameOrAlias(type);
        //If type is defined in another type
        if (type.DeclaringType is Type declaringType)
        {
            return $"{FormatTypeName(declaringType)}.{name}";
        }
        return name;
    }

    public static Type[] GetContainingTypes(this Type type)
    {
        if (type.IsArray)
            return new[] { type.GetElementType() ?? type };

        if (type.IsGenericType && type.IsConstructedGenericType)
            return type.GetGenericArguments();

        return new[] { type }; ;

    }
}
