using CSharpClientGenerator.Entities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static CSharpClientGenerator.Extensions.SyntaxSugar;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Microsoft.CodeAnalysis.CSharp.SyntaxKind;

namespace CSharpClientGenerator.Extensions;

public static class RecordDeclarationExt
{
    public static string ToCSharpCode(this RecordDeclaration record, string package)
    {
        var compilationUnit = Generate(record, package);
        return compilationUnit.NormalizeWhitespace().ToFullString();
    }

    private static CompilationUnitSyntax Generate(RecordDeclaration record, string package)
    {
        var parameters = record.Parameters.Select(p =>
                Parameter(Identifier(p.Name)).WithType(IdentifierName(p.Type.ToString())));
        
        return CompilationUnit()
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(FileScopedNamespaceDeclaration(Name(package))
                    .WithMembers(
                        SingletonList<MemberDeclarationSyntax>(
                            RecordDeclaration(Token(RecordKeyword), Identifier(record.Name))
                                .WithModifiers(Public)
                                .WithParameterList(ParameterList(SeparatedList(parameters)))
                                .WithSemicolonToken(Token(SemicolonToken))))));
    }
}