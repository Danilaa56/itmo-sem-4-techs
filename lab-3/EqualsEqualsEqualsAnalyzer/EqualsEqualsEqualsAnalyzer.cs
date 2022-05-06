using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EqualsEqualsEqualsAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class EqualsEqualsEqualsAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor DiagnosticDescriptor =
            new DiagnosticDescriptor(
                "EqualsEqualsEqualsAnalyzer",
                "Operator '==' does not defined for used types",
                "Operator '==' does not defined for these types",
                "Comparison",
                DiagnosticSeverity.Warning,
                true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(DiagnosticDescriptor);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeExpression, SyntaxKind.EqualsExpression);
        }

        private void AnalyzeExpression(SyntaxNodeAnalysisContext context)
        {
            var equalsExpression = (BinaryExpressionSyntax) context.Node;
            
            var symbol = context.SemanticModel.GetSymbolInfo(equalsExpression).Symbol;

            if (symbol is not IMethodSymbol methodSymbol)
                return;

            if (GetFullMethodName(methodSymbol) != "System.Object.op_Equality")
                return;
            
            context.ReportDiagnostic(
                Diagnostic.Create(
                    DiagnosticDescriptor,
                    equalsExpression.GetLocation()));
        }

        private string GetFullMethodName(IMethodSymbol symbol)
        {
            return GetFullTypeName(symbol.ContainingType) + "." + symbol.Name;
        }
        
        private string GetFullTypeName(ITypeSymbol symbol)
        {
            if (symbol.ContainingType != null)
            {
                return GetFullTypeName(symbol.ContainingType) + "." + symbol.Name;
            }
        
            if (symbol.ContainingNamespace.IsGlobalNamespace)
            {
                return symbol.Name;
            }
        
            return symbol.ContainingNamespace + "." + symbol.Name;
        }
    }
}