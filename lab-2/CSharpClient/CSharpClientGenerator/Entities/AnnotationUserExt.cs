namespace CSharpClientGenerator.Entities;

public static class AnnotationUserExt
{
    public static bool HasAnnotation(this IAnnotationUser obj, string annotationName)
    {
        return obj.Annotations.Any(a => annotationName.Equals(a.Name));
    }

    public static Annotation AnnotationByName(this IAnnotationUser obj, string annotationName)
    {
        return obj.Annotations.First(a => annotationName.Equals(a.Name));
    }
}