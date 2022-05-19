using CSharpClientGenerator.Entities;
using static JavaParser;

namespace CSharpClientGenerator.ParserUtils;

public static class ListenerSugar
{
    public static RecordDeclaration Parse(this RecordDeclarationContext context)
    {
        var annotations = ((TypeDeclarationContext)context.Parent).classOrInterfaceModifier().ParseAnnotations();
        var name = context.identifier().GetText();
        var parameters = context.recordHeader().Parse();
        return new RecordDeclaration(annotations, name, parameters);
    }
    
    public static ClassDeclaration Parse(this ClassDeclarationContext context)
    {
        var annotations = ((TypeDeclarationContext)context.Parent).classOrInterfaceModifier().ParseAnnotations();
        var name = context.identifier().GetText();
        var methods = context.classBody().classBodyDeclaration().ParseMethods();
        return new ClassDeclaration(annotations, name, methods);
    }

    public static IEnumerable<Parameter> Parse(this RecordHeaderContext context)
    {
        if (context.recordComponentList() is null)
            return Array.Empty<Parameter>();
        return context.recordComponentList().recordComponent().Select(r => r.Parse());
    }
    
    public static IEnumerable<MethodDeclaration> ParseMethods(this IEnumerable<ClassBodyDeclarationContext> context)
    {
        return context.Select(b => b.memberDeclaration())
            .Where(m => m.methodDeclaration() is not null)
            .Select(m => m.methodDeclaration().Parse());
    }

    public static MethodDeclaration Parse(this MethodDeclarationContext context)
    {
        var classBodyDeclaration = (ClassBodyDeclarationContext)context.Parent.Parent;
        var annotations = classBodyDeclaration.modifier().ParseAnnotations();
        var type = context.typeTypeOrVoid().Parse();
        var name = context.identifier().GetText();
        var parameters = context.formalParameters().Parse();
        return new MethodDeclaration(annotations, type, name, parameters);
    }

    public static IEnumerable<Annotation> ParseAnnotations(this IEnumerable<VariableModifierContext>? context)
    {
        if (context is null)
            return Array.Empty<Annotation>();
        return context.Where(m => m.annotation() is not null)
            .Select(m => m.annotation().Parse());
    }
    
    public static IEnumerable<Annotation> ParseAnnotations(this IEnumerable<ModifierContext>? context)
    {
        if (context is null)
            return Array.Empty<Annotation>();
        return context.Where(m => m.classOrInterfaceModifier() is not null)
            .Select(m => m.classOrInterfaceModifier())
            .ParseAnnotations();
    }

    public static IEnumerable<Annotation> ParseAnnotations(
        this IEnumerable<ClassOrInterfaceModifierContext>? context)
    {
        if (context is null)
            return Array.Empty<Annotation>();
        return context.Where(m => m.annotation() is not null)
            .Select(m => m.annotation().Parse());
    }

    public static IEnumerable<Parameter> Parse(this FormalParametersContext context)
    {
        if (context.formalParameterList() is null)
            return Array.Empty<Parameter>();
        return context.formalParameterList().Parse();
    }

    public static IEnumerable<Parameter> Parse(this FormalParameterListContext context)
    {
        return context.formalParameter().Select(p => p.Parse());
    }

    public static IEnumerable<Parameter> Parse(this RecordComponentListContext context)
    {
        return context.recordComponent().Select(r => r.Parse());
    }

    public static Parameter Parse(this FormalParameterContext context)
    {
        var annotations = context.variableModifier().ParseAnnotations();
        var type = context.typeType().Parse().WithAnnotations(annotations);
        return new Parameter(type, context.variableDeclaratorId().GetText());
    }

    public static Parameter Parse(this RecordComponentContext context)
    {
        return new Parameter(context.typeType().Parse(), context.identifier().GetText());
    }

    public static TypeType Parse(this TypeTypeOrVoidContext context)
    {
        if (context.typeType() is not null)
            return context.typeType().Parse();
        return new TypeType(new List<Annotation>(), "void");
    }

    public static TypeType Parse(this TypeTypeContext context)
    {
        var annotations = context.annotation().Select(a => a.Parse());
        string name;
        IEnumerable<TypeType>? nestedTypes;

        var classOrInterfaceTypeContext = context.classOrInterfaceType();
        if (classOrInterfaceTypeContext is not null)
        {
            var classOrInterface = classOrInterfaceTypeContext.Parse();
            name = classOrInterface.TypeName;
            nestedTypes = classOrInterface.NestedTypes;
        }
        else
        {
            name = context.primitiveType().GetText();
            nestedTypes = null;
        }

        return new TypeType(annotations, name, nestedTypes);
    }

    public static ClassOrInterfaceType Parse(this ClassOrInterfaceTypeContext context)
    {
        var name = context.identifier()[0].GetText();

        if (context.typeArguments() is null || context.typeArguments().Length == 0)
        {
            return new ClassOrInterfaceType(name);
        }

        var typeArguments = context.typeArguments()[0].Parse();
        return new ClassOrInterfaceType(name, typeArguments);
    }

    public static IEnumerable<TypeType> Parse(this TypeArgumentsContext context)
    {
        return context.typeArgument().Select(a => a.typeType().Parse());
    }

    public static Annotation Parse(this AnnotationContext context)
    {
        var name = context.qualifiedName().GetText();
        if (context.elementValue() is null)
            return new Annotation(name, null);
        return new Annotation(name, context.elementValue().GetText());
    }
}