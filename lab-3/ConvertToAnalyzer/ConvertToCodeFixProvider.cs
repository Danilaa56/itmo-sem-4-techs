using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ConvertToAnalyzer
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ConvertToCodeFixProvider))]
    [Shared]
    public class ConvertToCodeFixProvider : CodeFixProvider
    {
        private static readonly Dictionary<string, SyntaxKind> KeyWordsMap = CreateKeyWordsMap();

        public override ImmutableArray<string> FixableDiagnosticIds { get => new ConvertToAnalyzer().SupportedDiagnostics.Select(d => d.Id).ToImmutableArray(); }

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var diagnostic = context.Diagnostics.First();

            var document = context.Document;
            var root = await document.GetSyntaxRootAsync();
            var invocationExpression = (InvocationExpressionSyntax) root.FindNode(diagnostic.Location.SourceSpan);

            context.RegisterCodeFix(CodeAction.Create("Replace Convert.To with .Parse syntax",
                    ignored => {
                        var methodName = ((MemberAccessExpressionSyntax) invocationExpression.Expression).Name.ToString();
                        var typeName = methodName.Substring(2);

                        ExpressionSyntax typeIdentifier;
                        if (KeyWordsMap.TryGetValue(typeName, out var syntaxKind))
                        {
                            typeIdentifier = SyntaxFactory.PredefinedType(SyntaxFactory.Token(syntaxKind));
                        }
                        else
                        {
                            typeIdentifier = SyntaxFactory.IdentifierName(typeName);
                        }

                        var parseIdentifier = SyntaxFactory.IdentifierName("Parse");
                        var parseMethod = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, typeIdentifier, parseIdentifier);
                        var parseInvocation = SyntaxFactory.InvocationExpression(parseMethod, invocationExpression.ArgumentList);

                        root = root.ReplaceNode(invocationExpression, parseInvocation);

                        return Task.FromResult(document.WithSyntaxRoot(root));
                    }),
                diagnostic);
        }

        private static Dictionary<string, SyntaxKind> CreateKeyWordsMap()
        {
            var map = new Dictionary<string, SyntaxKind>
            {
                ["Boolean"] = SyntaxKind.BoolKeyword,
                ["Char"] = SyntaxKind.CharKeyword,
                ["SByte"] = SyntaxKind.SByteKeyword,
                ["Byte"] = SyntaxKind.ByteKeyword,
                ["Int16"] = SyntaxKind.ShortKeyword,
                ["UInt16"] = SyntaxKind.UShortKeyword,
                ["Int32"] = SyntaxKind.IntKeyword,
                ["UInt32"] = SyntaxKind.UIntKeyword,
                ["Int64"] = SyntaxKind.LongKeyword,
                ["UInt64"] = SyntaxKind.ULongKeyword,
                ["Single"] = SyntaxKind.FloatKeyword,
                ["Double"] = SyntaxKind.DoubleKeyword,
                ["Decimal"] = SyntaxKind.DecimalKeyword
            };
            return map;
        }
    }
}