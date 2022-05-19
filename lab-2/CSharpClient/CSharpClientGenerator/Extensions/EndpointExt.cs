using CSharpClientGenerator.Entities.Endpoints;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static CSharpClientGenerator.Extensions.SyntaxSugar;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace CSharpClientGenerator.Extensions;

public static class EndpointExt
{
    private static readonly SyntaxTokenList ModifierPublic = TokenList(Token(SyntaxKind.PublicKeyword));

    public static MemberDeclarationSyntax ToMethodDeclaration(this Endpoint endpoint)
    {
        // public Text Add
        var methodDeclaration = MethodDeclaration(Name(endpoint.ReturnType), Identifier(endpoint.Name))
            .WithModifiers(ModifierPublic);

        // (Text text)
        methodDeclaration = methodDeclaration.WithParameterList(endpoint.Parameters.ToParametersList());

        var bodyStatements = new List<StatementSyntax>
        {
            // using var client = new HttpClient();
            VarHttpClientStatement(),

            // var url = Url + id + "/";
            VarUrl(endpoint),

            // var response = client.PostAsJsonAsync(Url, text).Result;
            VarResponse(endpoint),

            // if (!response.IsSuccessStatusCode) throw...
            ResponseCodeCheck()
        };

        if (!"void".Equals(endpoint.ReturnType))
        {
            // return response.Content.ReadFromJsonAsync<Text>().Result;
            bodyStatements.Add(Return(endpoint.ReturnType));
        }

        methodDeclaration = methodDeclaration.WithBody(Block(bodyStatements));

        return methodDeclaration;
    }

    private static StatementSyntax Return(string returnType)
    {
        return ReturnStatement(
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        Access("response", "Content"),
                        GenericName(Identifier("ReadFromJsonAsync"))
                            .WithTypeArgumentList(
                                TypeArgumentList(
                                    SingletonSeparatedList<TypeSyntax>(
                                        IdentifierName(returnType)))))),
                IdentifierName("Result")));
    }

    private static StatementSyntax ResponseCodeCheck()
    {
        return IfStatement(
            PrefixUnaryExpression(
                SyntaxKind.LogicalNotExpression,
                Access("response", "IsSuccessStatusCode")),
            Block(
                SingletonList<StatementSyntax>(
                    ThrowStatement(
                        ObjectCreationExpression(
                                IdentifierName("IOException"))
                            .WithArgumentList(
                                ArgumentList(
                                    SingletonSeparatedList(
                                        Argument(
                                            InterpolatedStringExpression(
                                                    Token(SyntaxKind.InterpolatedStringStartToken))
                                                .WithContents(
                                                    List(new InterpolatedStringContentSyntax[]
                                                    {
                                                        InterpolatedStringText()
                                                            .WithTextToken(
                                                                Token(
                                                                    TriviaList(),
                                                                    SyntaxKind.InterpolatedStringTextToken,
                                                                    "HTTP Request failed, response code: ",
                                                                    "HTTP Request failed, response code: ",
                                                                    TriviaList())),
                                                        Interpolation(Access("response", "StatusCode"))
                                                    }))))))))));
    }

    private static StatementSyntax VarResponse(Endpoint endpoint)
    {
        var methodName = endpoint.Type switch
        {
            RequestType.Get => "GetAsync",
            RequestType.Post => "PostAsJsonAsync",
            RequestType.Put => "PutAsJsonAsync",
            RequestType.Delete => "DeleteAsync",
            _ => throw new ArgumentOutOfRangeException()
        };

        var argumentList = ArgumentList();
        argumentList = argumentList.AddArguments(Argument(IdentifierName("url")));
        if (endpoint.IsPostOrPut())
            argumentList = argumentList.AddArguments(Argument(IdentifierName(endpoint.Body!.Name)));

        return LocalVarInit("response",
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                Invoke("client", methodName)
                    .WithArgumentList(argumentList),
                IdentifierName("Result")));
    }

    private static StatementSyntax VarUrl(Endpoint endpoint)
    {
        var addSequence = AddSequence(endpoint.Parts.Select(part => part.ToExpressionSyntax()));
        return LocalVarInit("url", addSequence);
    }

    private static LocalDeclarationStatementSyntax VarHttpClientStatement()
    {
        return LocalVarInit("client",
                ObjectCreationExpression(IdentifierName("HttpClient"))
                    .WithArgumentList(ArgumentList()))
            .WithUsingKeyword(Token(SyntaxKind.UsingKeyword));
    }

    private static ParameterListSyntax ToParametersList(this IEnumerable<EndpointParameter> parameters)
    {
        return ParameterList(SeparatedList(
            parameters.Select(p =>
                Parameter(Identifier(p.Name)).WithType(IdentifierName(p.Type)))));
    }
}