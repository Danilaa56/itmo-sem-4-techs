using System.Collections.Immutable;
using System.Text;

namespace CSharpClientGenerator.Entities;

public sealed class MethodDeclaration : IAnnotationUser, ITypeMentioner
{
    public ImmutableArray<Annotation> Annotations { get; }
    public readonly TypeType ReturnType;
    public string Name;
    public ImmutableArray<Parameter> Parameters;

    public MethodDeclaration(
        IEnumerable<Annotation> annotations,
        TypeType returnType,
        string name,
        IEnumerable<Parameter> parameters)
    {
        Annotations = annotations.ToImmutableArray();
        ReturnType = returnType;
        Name = name;
        Parameters = parameters.ToImmutableArray();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var annotation in Annotations)
        {
            sb.AppendLine(annotation.ToString());
        }

        sb.Append($"{ReturnType} {Name} (");
        for (var i = 0; i < Parameters.Length; i++)
        {
            if (i == 0)
                sb.Append(Parameters[i]);
            else
                sb.Append(", ").Append(Parameters[i]);
        }

        sb.Append(") { }");
        return sb.ToString();
    }

    public HashSet<TypeType> GetAllTypeLinks()
    {
        var links = ReturnType.GetAllTypeLinks();
        foreach (var t in Parameters.SelectMany(p => p.GetAllTypeLinks()))
        {
            links.Add(t);
        }

        return links;
    }
}