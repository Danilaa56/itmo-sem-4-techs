using CSharpClientGenerator.Entities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static CSharpClientGenerator.Extensions.SyntaxSugar;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace CSharpClientGenerator.Extensions;

public static class ClassDeclarationExt
{
    public static string ToCSharpCode(this ClassDeclaration controller, string package, IEnumerable<string> usings)
    {
        var compilationUnit = Generate(controller, package, usings);
        return compilationUnit.NormalizeWhitespace().ToFullString();
    }

    private static CompilationUnitSyntax Generate(ClassDeclaration controller, string package, IEnumerable<string> usings)
    {
        var members = new List<MemberDeclarationSyntax>
        {
            MappingField(controller),
            UrlField(),
            Constructor(controller)
        };
        members.AddRange(controller.MethodDeclarations.Select(method => method.ToEndpoint().ToMethodDeclaration()));

        return CompilationUnit()
            .WithUsings(usings.Reverse().Append("System.Net.Http.Json").Reverse())
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    FileScopedNamespaceDeclaration(Name(package))
                        .WithMembers(
                            SingletonList<MemberDeclarationSyntax>(
                                ClassDeclaration(controller.Name)
                                    .WithModifiers(Public)
                                    .WithMembers(List(members))))));
    }

    private static FieldDeclarationSyntax MappingField(ClassDeclaration controller)
    {
        var mappingValue = controller.AnnotationByName("RequestMapping").Value + "/";
        return FieldDeclaration(
                StringDeclaration()
                    .WithVariable(VariableDeclarator(Identifier("Mapping"))
                            .WithInitializer(EqualsValueClause(StringLiteralExpression(mappingValue)))))
            .WithModifiers(
                TokenList(
                    new[]
                    {
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.ConstKeyword)
                    }));
    }

    private static FieldDeclarationSyntax UrlField()
    {
        return FieldDeclaration(
                StringDeclaration().WithVariable(VariableDeclarator(Identifier("Url"))))
            .WithModifiers(
                TokenList(
                    new[]
                    {
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.ReadOnlyKeyword)
                    }));
    }

    private static ConstructorDeclarationSyntax Constructor(ClassDeclaration controller)
    {
        return ConstructorDeclaration(Identifier(controller.Name))
            .WithModifiers(Public)
            .WithParameterList(
                ParameterList(
                    SingletonSeparatedList(
                        Parameter(Identifier("baseUrl"))
                            .WithType(PredefinedType(Token(SyntaxKind.StringKeyword))))))
            .WithBody(
                Block(
                    SingletonList<StatementSyntax>(
                        ExpressionStatement(
                            AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                IdentifierName("Url"),
                                BinaryExpression(
                                    SyntaxKind.AddExpression,
                                    IdentifierName("baseUrl"),
                                    IdentifierName("Mapping")))))));
    }
}