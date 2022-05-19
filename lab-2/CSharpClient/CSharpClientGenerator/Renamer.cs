using CSharpClientGenerator.Entities;

namespace CSharpClientGenerator;

public sealed class JavaToCSharpRenamer
{
    private readonly Dictionary<string, string> _typeMap;

    public JavaToCSharpRenamer()
    {
        _typeMap = new Dictionary<string, string>
        {
            { "boolean", "bool" },
            { "Boolean", "bool" },
            { "Character", "char" },
            { "Byte", "byte" },
            { "Short", "short" },
            { "Integer", "int" },
            { "Float", "float" },
            { "Double", "double" },
            { "String", "string" },
            { "Object", "object" },
            { "UUID", "Guid" },
            { "Collection", "IEnumerable" },
            { "List", "IEnumerable" },
            { "LinkedList", "IEnumerable" },
            { "Set", "IEnumerable" },
            { "Date", "DateTime" },
        };
    }

    public void RenameTypeLinks(IEnumerable<TypeType> types)
    {
        foreach (var type in types)
        {
            if (_typeMap.TryGetValue(type.Name, out var newName))
            {
                type.Name = newName;
            }
        }
    }

    public void RenameMethods(IEnumerable<MethodDeclaration> methods)
    {
        foreach (var method in methods)
        {
            method.Name = FirstLetterUpperCase(method.Name);
        }
    }

    private static string FirstLetterUpperCase(string str)
    {
        var chars = str.ToCharArray();
        chars[0] = char.ToUpper(chars[0]);
        return new string(chars);
    }
}