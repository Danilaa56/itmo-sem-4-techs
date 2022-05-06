using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ConvertToAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ConvertToAnalyzer : DiagnosticAnalyzer
    {
        private static readonly DiagnosticDescriptor DiagnosticDescriptor =
            new DiagnosticDescriptor(
                "ConvertToAnalyzer",
                "Using Convert.ToType instead of Type.Parse",
                "Convert.ToType can be replaced with Type.Parse",
                "Convertion",
                DiagnosticSeverity.Warning,
                true);

        private static readonly string[] MethodNames = new string[] {"Boolean", "Char", "SByte", "Byte", "Int16", "UInt16", "Int32", "UInt32", "Int64", "UInt64", "Single", "Double", "Decimal", "DateTime"}
            .Select(typeName => "To" + typeName)
            .ToArray();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(DiagnosticDescriptor);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeExpression, SyntaxKind.InvocationExpression);
        }

        private static void AnalyzeExpression(SyntaxNodeAnalysisContext context)
        {
            var invocationExpression = (InvocationExpressionSyntax) context.Node;

            if (invocationExpression.ArgumentList.Arguments.Count != 1)
                return;

            if (!(context.SemanticModel.GetSymbolInfo(invocationExpression.Expression).Symbol is IMethodSymbol methodSymbol))
                return;

            if (methodSymbol.ContainingType.ToString() != "System.Convert")
                return;

            if (!MethodNames.Contains(methodSymbol.Name))
                return;

            if (GetFullTypeName(methodSymbol.Parameters[0].Type) != "System.String")
                return;

            context.ReportDiagnostic(
                Diagnostic.Create(
                    DiagnosticDescriptor,
                    invocationExpression.GetLocation()));
        }

        private static string GetFullTypeName(ITypeSymbol symbol)
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