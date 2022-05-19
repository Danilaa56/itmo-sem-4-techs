using CSharpClientGenerator.Entities.Endpoints;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpClientGenerator.Extensions;

public static class UrlPartExt
{
    public static ExpressionSyntax ToExpressionSyntax(this UrlPart part)
    {
        return part.Type switch
        {
            UrlPartType.Var => SyntaxFactory.IdentifierName(part.Str),
            UrlPartType.Literal => SyntaxSugar.StringLiteralExpression(part.Str),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}