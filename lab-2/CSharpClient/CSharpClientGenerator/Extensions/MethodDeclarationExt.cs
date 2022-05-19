using System.Text.RegularExpressions;
using CSharpClientGenerator.Entities;
using CSharpClientGenerator.Entities.Endpoints;

namespace CSharpClientGenerator.Extensions;

public static class MethodDeclarationExt
{
    public static Endpoint ToEndpoint(this MethodDeclaration method)
    {
        var mappingAnnotation = GetMappingAnnotation(method);
        var pathParameters = GetPathParameters(method);
        var bodyParameter = FindBodyParameter(method);

        var type = GetRequestType(mappingAnnotation.Name);

        var urlParts = GetUrlParts(mappingAnnotation.Value, pathParameters);

        return new Endpoint(
            type,
            urlParts,
            method.ReturnType.ToString(),
            method.Name,
            method.Parameters.Select(p => new EndpointParameter(p.Type.ToFullString(), p.Name)),
            bodyParameter);
    }

    private static IEnumerable<UrlPart> GetUrlParts(string? mappingUrl, List<Parameter> pathParameters)
    {
        var parts = new List<UrlPart> { new(UrlPartType.Var, "Url") };

        if (mappingUrl is null)
        {
            if (pathParameters.Count != 0)
                throw new ArgumentException("Mapping url does not require path variables, but they exit");
            return parts;
        }

        // Regex.Matches(mappingUrl, '{' +  + '}')
        var usedPathParameters = new HashSet<string>();

        parts.Add(new UrlPart(UrlPartType.Literal, mappingUrl));
        parts = pathParameters.Aggregate(parts, (current, parameter) => current.SelectMany(part =>
                {
                    var list = new List<UrlPart>();
                    if (part.Type == UrlPartType.Var)
                    {
                        list.Add(part);
                    }
                    else
                    {
                        var subParts = part.Str.Split("{" + parameter.Name + "}");
                        list.Add(new UrlPart(UrlPartType.Literal, subParts[0]));
                        for (var i = 1; i < subParts.Length; i++)
                        {
                            list.Add(new UrlPart(UrlPartType.Var, parameter.Name));
                            list.Add(new UrlPart(UrlPartType.Literal, subParts[i]));
                            usedPathParameters.Add(parameter.Name);
                        }
                    }

                    return list;
                })
                .ToList())
            .Where(part => part.Type != UrlPartType.Literal || part.Str.Length > 0)
            .ToList();

        foreach (var pathParameter in pathParameters.Where(pathParameter => !usedPathParameters.Contains(pathParameter.Name)))
        {
            throw new ArgumentException(
                $"Path parameter '{pathParameter.Name}' was not found in mapping url '{mappingUrl}'");
        }

        foreach (var literalPart in parts.Where(part => part.Type == UrlPartType.Literal))
        {
            var match = Regex.Match(literalPart.Str, "\\{.*\\}");
            if (match.Success)
            {
                throw new ArgumentException(
                    $"Mapping url {mappingUrl} contains spare path variable '{match.Value[1..^1]}'");
            }
        }

        return parts;
    }

    private static RequestType GetRequestType(string mappingName)
    {
        return mappingName[..^7] switch
        {
            "Get" => RequestType.Get,
            "Post" => RequestType.Post,
            "Put" => RequestType.Put,
            "Delete" => RequestType.Delete,
            _ => throw new ArgumentException("Couldn't get request type from mapping name: " + mappingName)
        };
    }

    private static EndpointParameter? FindBodyParameter(MethodDeclaration method)
    {
        var parameter = method.Parameters.FirstOrDefault(p => p.Type.HasAnnotation("RequestBody"));
        return parameter is null ? null : new EndpointParameter(parameter.Type.ToString(), parameter.Name);
    }

    private static List<Parameter> GetPathParameters(MethodDeclaration method)
    {
        return method.Parameters
            .Where(p => p.Type.HasAnnotation("PathVariable"))
            .ToList();
    }

    private static Annotation GetMappingAnnotation(MethodDeclaration method)
    {
        return method.Annotations.First(a => a.Name.Contains("Mapping"));
    }
}