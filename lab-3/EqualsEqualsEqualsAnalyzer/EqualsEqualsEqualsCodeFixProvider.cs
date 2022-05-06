using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EqualsEqualsEqualsAnalyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(EqualsEqualsEqualsCodeFixProvider))]
    [Shared]
    public class EqualsEqualsEqualsCodeFixProvider : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds { get => new EqualsEqualsEqualsAnalyzer().SupportedDiagnostics.Select(d => d.Id).ToImmutableArray(); }

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var diagnostic = context.Diagnostics.First();

            var document = context.Document;
            var root = await document.GetSyntaxRootAsync();
            var semanticModel = await document.GetSemanticModelAsync();
            var equalsExpression = (BinaryExpressionSyntax) root!.FindNode(diagnostic.Location.SourceSpan);

            context.RegisterCodeFix(CodeAction.Create("Replace operator '==' with .Equals() call",
                    ignored => {
                        var leftType = semanticModel.GetTypeInfo(equalsExpression.Left).Type;

                        var equalsName = SyntaxFactory.IdentifierName("Equals");
                        var equalsMethod = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, equalsExpression.Left, equalsName);
                        var argument = SyntaxFactory.Argument(equalsExpression.Right);
                        var argumentList = SyntaxFactory.ArgumentList().AddArguments(argument);
                        var equalsInvocation = SyntaxFactory.InvocationExpression(equalsMethod, argumentList);
                        
                        root = root.ReplaceNode(equalsExpression, equalsInvocation);

                        return Task.FromResult(document.WithSyntaxRoot(root));
                    }),
                diagnostic);
        }
    }
}