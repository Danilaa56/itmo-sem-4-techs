using System.Collections.Immutable;
using System.Text;

namespace CSharpClientGenerator.Entities;

public sealed class ClassDeclaration : IAnnotationUser, ITypeMentioner
{
    public ImmutableArray<Annotation> Annotations { get; }
    public string Name;
    public ImmutableArray<MethodDeclaration> MethodDeclarations;

    public ClassDeclaration(IEnumerable<Annotation> annotations, string name, IEnumerable<MethodDeclaration> methodDeclarations)
    {
        Annotations = annotations.ToImmutableArray();
        Name = name;
        MethodDeclarations = methodDeclarations.ToImmutableArray();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var annotation in Annotations)
        {
            sb.AppendLine(annotation.ToString());
        }

        sb.Append("class " + Name + " {");
        foreach (var method in MethodDeclarations)
        {
            sb.AppendLine();
            foreach (var line in method.ToString().Split("\n"))
            {
                sb.AppendLine("\t" + line);
            }
        }

        sb.Append('}');
        return sb.ToString();
    }

    public HashSet<TypeType> GetAllTypeLinks()
    {
        return MethodDeclarations.SelectMany(m => m.GetAllTypeLinks()).ToHashSet();
    }
}