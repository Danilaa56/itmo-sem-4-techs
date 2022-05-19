using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace CSharpClientGenerator.Extensions;

public static class SyntaxSugar
{
    public static SyntaxTokenList Public = TokenList(Token(SyntaxKind.PublicKeyword));
    
    public static VariableDeclarationSyntax StringDeclaration()
    {
        return VariableDeclaration(PredefinedType(Token(SyntaxKind.StringKeyword)));
    }

    public static VariableDeclarationSyntax VarInit(string typeName, string varName, ExpressionSyntax valueExpression)
    {
        return VariableDeclaration(
            IdentifierName(typeName),
            SingletonSeparatedList(VariableDeclarator(Identifier(varName))
                .WithInitializer(EqualsValueClause(valueExpression))));
    }

    public static LocalDeclarationStatementSyntax LocalVarInit(string varName, ExpressionSyntax valueExpression)
    {
        return LocalDeclarationStatement(VarInit("var", varName, valueExpression));
    }

    public static LiteralExpressionSyntax StringLiteralExpression(string str)
    {
        return LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(str));
    }

    public static ExpressionSyntax AddSequence(IEnumerable<ExpressionSyntax> expressions)
    {
        using var enumerator = expressions.GetEnumerator();
        while (enumerator.Current is null && enumerator.MoveNext())
        {
        }

        var expression = enumerator.Current;
        while (enumerator.MoveNext())
            expression = BinaryExpression(SyntaxKind.AddExpression, expression!, enumerator.Current!);
        return expression!;
    }

    public static InvocationExpressionSyntax Invoke(string varName, string methodName)
    {
        return InvocationExpression(Access(varName, methodName));
    }

    public static MemberAccessExpressionSyntax Access(string varName, string name)
    {
        return MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            IdentifierName(varName),
            IdentifierName(name));
    }

    public static NameSyntax Name(string name)
    {
        var subNames = name.Split(".").Select(IdentifierName).ToArray();
        NameSyntax identifierName = subNames[0];
        for (var i = 1; i < subNames.Length; i++)
            identifierName = QualifiedName(identifierName, subNames[i]);

        return identifierName;
    }

    public static CompilationUnitSyntax WithUsings(this CompilationUnitSyntax compilationUnit,
        IEnumerable<string> packages)
    {
        return compilationUnit.WithUsings(List(packages
            .Select(Name)
            .Select(UsingDirective)
            .ToList()));
    }

    public static VariableDeclarationSyntax WithVariable(
        this VariableDeclarationSyntax declaration,
        VariableDeclaratorSyntax declarator)
    {
        return declaration.WithVariables(SingletonSeparatedList(declarator));
    }
}