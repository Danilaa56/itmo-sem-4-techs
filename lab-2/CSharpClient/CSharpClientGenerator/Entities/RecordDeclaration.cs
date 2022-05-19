using System.Collections.Immutable;
using System.Text;

namespace CSharpClientGenerator.Entities;

public sealed class RecordDeclaration : IAnnotationUser, ITypeMentioner
{
    public ImmutableArray<Annotation> Annotations { get; }
    public readonly string Name;
    public ImmutableArray<Parameter> Parameters;

    public RecordDeclaration(IEnumerable<Annotation> annotations, string name, IEnumerable<Parameter> parameters)
    {
        Annotations = annotations.ToImmutableArray();
        Name = name;
        Parameters = parameters.ToImmutableArray();
    }

    public HashSet<TypeType> GetAllTypeLinks()
    {
        return Parameters.SelectMany(p => p.GetAllTypeLinks()).ToHashSet();
    }
    
    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var annotation in Annotations)
        {
            sb.AppendLine(annotation.ToString());
        }

        sb.Append("record " + Name + " (");
        for (var i = 0; i < Parameters.Length; i++)
        {
            if (i == 0)
                sb.Append(Parameters[i]);
            else
                sb.Append(", ").Append(Parameters[i]);
        }

        sb.Append(");");
        return sb.ToString();
    }
}