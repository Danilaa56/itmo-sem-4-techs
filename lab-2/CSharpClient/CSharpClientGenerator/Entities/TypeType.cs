using System.Collections.Immutable;
using System.Text;

namespace CSharpClientGenerator.Entities;

public sealed class TypeType : IAnnotationUser, ITypeMentioner
{
    public ImmutableArray<Annotation> Annotations { get; }
    public string Name { get; set; }
    private readonly List<TypeType>? _nestedTypes;

    public TypeType(IEnumerable<Annotation> annotations, string name, IEnumerable<TypeType>? nestedTypes = null)
    {
        Annotations = annotations.ToImmutableArray();
        Name = name;
        if (nestedTypes is not null)
            _nestedTypes = nestedTypes.ToList();
    }

    public TypeType WithAnnotations(IEnumerable<Annotation> annotations)
    {
        return new TypeType(annotations, Name, _nestedTypes);
    }

    public string ToFullString()
    {
        var sb = new StringBuilder();
        sb.Append(Name);

        if (_nestedTypes is null)
            return sb.ToString();

        sb.Append('<');
        if (_nestedTypes.Count > 0)
            sb.Append(_nestedTypes[0].ToFullString());
        for (var i = 1; i < _nestedTypes.Count; i++)
        {
            sb.Append(", ").Append(_nestedTypes[i].ToFullString());
        }

        sb.Append('>');

        return sb.ToString();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var annotation in Annotations)
        {
            sb.Append(annotation).Append(' ');
        }

        sb.Append(Name);

        if (_nestedTypes is null)
            return sb.ToString();

        sb.Append('<');
        if (_nestedTypes.Count > 0)
            sb.Append(_nestedTypes[0]);
        for (var i = 1; i < _nestedTypes.Count; i++)
        {
            sb.Append(", ").Append(_nestedTypes[i]);
        }

        sb.Append('>');

        return sb.ToString();
    }

    public HashSet<TypeType> GetAllTypeLinks()
    {
        var links = new HashSet<TypeType> { this };
        if (_nestedTypes is null)
            return links;

        foreach (var t in _nestedTypes.SelectMany(nestedType => nestedType.GetAllTypeLinks()))
            links.Add(t);

        return links;
    }
}