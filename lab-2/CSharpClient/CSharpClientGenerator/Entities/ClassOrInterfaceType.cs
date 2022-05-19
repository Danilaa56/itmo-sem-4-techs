using System.Collections.Immutable;

namespace CSharpClientGenerator.Entities;

public sealed class ClassOrInterfaceType
{
    public readonly string TypeName;
    public readonly ImmutableList<TypeType>? NestedTypes;

    public ClassOrInterfaceType(string typeName, IEnumerable<TypeType>? nestedTypes = null)
    {
        TypeName = typeName;
        if (nestedTypes is not null)
            NestedTypes = nestedTypes.ToImmutableList();
    }
}