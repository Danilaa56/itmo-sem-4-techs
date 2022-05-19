using System.Collections.Immutable;

namespace CSharpClientGenerator.Entities;

public sealed class Parameter : IAnnotationUser, ITypeMentioner
{
    public ImmutableArray<Annotation> Annotations => Type.Annotations;
    public TypeType Type { get; }
    public string Name { get; }

    public Parameter(TypeType type, string name)
    {
        Type = type;
        Name = name;
    }
    
    public override string ToString()
    {
        return $"{Type} {Name}";
    }

    public HashSet<TypeType> GetAllTypeLinks()
    {
        return Type.GetAllTypeLinks();
    }
}