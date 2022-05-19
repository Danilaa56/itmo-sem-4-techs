using System.Collections.Immutable;

namespace CSharpClientGenerator.Entities;

public interface IAnnotationUser
{ 
    public ImmutableArray<Annotation> Annotations { get; }
}