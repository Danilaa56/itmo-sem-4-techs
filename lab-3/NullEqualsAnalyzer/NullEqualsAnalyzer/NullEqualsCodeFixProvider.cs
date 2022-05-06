using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NullEqualsAnalyzer;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NullEqualsCodeFixProvider))]
[Shared]
public class NullEqualsCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds { get => new NullEqualsAnalyzer().SupportedDiagnostics.Select(d => d.Id).ToImmutableArray(); }

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics.First();

        var document = context.Document;

        var root = await document.GetSyntaxRootAsync();

        var equalsExpression = (BinaryExpressionSyntax) root!.FindNode(diagnostic.Location.SourceSpan);

        context.RegisterCodeFix(CodeAction.Create("Use 'is' expression",
                ignored => {
                    PatternSyntax isPattern = SyntaxFactory.ConstantPattern(equalsExpression.Right);

                    if (equalsExpression.Kind() == SyntaxKind.NotEqualsExpression)
                        isPattern = SyntaxFactory.UnaryPattern(SyntaxFactory.Token(SyntaxKind.NotKeyword), isPattern);

                    var isExpression = SyntaxFactory.IsPatternExpression(equalsExpression.Left, isPattern);

                    root = root.ReplaceNode(equalsExpression, isExpression);

                    return Task.FromResult(document.WithSyntaxRoot(root));
                }),
            diagnostic);
    }
}