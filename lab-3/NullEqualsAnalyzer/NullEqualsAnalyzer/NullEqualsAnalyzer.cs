using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace NullEqualsAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class NullEqualsAnalyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor DiagnosticDescriptor =
        new (
            "NullEquals",
            "Using equals equals expression operator with null",
            "Operator equals equals can be replaced with 'is' expression",
            "Operations with null",
            DiagnosticSeverity.Warning,
            true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(DiagnosticDescriptor);

    public override void Initialize(AnalysisContext context)
    {
        context.RegisterSyntaxNodeAction(AnalyzeEqualsExpression, SyntaxKind.EqualsExpression);
        context.RegisterSyntaxNodeAction(AnalyzeEqualsExpression, SyntaxKind.NotEqualsExpression);
    }

    private static void AnalyzeEqualsExpression(SyntaxNodeAnalysisContext context)
    {
        var equalsExpression = (BinaryExpressionSyntax) context.Node;

        var isLeftNull = equalsExpression.Left is LiteralExpressionSyntax leftLiteral
                         && leftLiteral.Kind() == SyntaxKind.NullLiteralExpression;
        var isRightNull = equalsExpression.Right is LiteralExpressionSyntax rightLiteral
                          && rightLiteral.Kind() == SyntaxKind.NullLiteralExpression;

        if (!isLeftNull && !isRightNull)
        {
            return;
        }

        var diagnostic = Diagnostic.Create(DiagnosticDescriptor, equalsExpression.OperatorToken.GetLocation());
        context.ReportDiagnostic(diagnostic);
    }
}