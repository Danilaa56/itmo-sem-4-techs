namespace CSharpClientGenerator.Entities.Endpoints;

public sealed class Endpoint
{
    public readonly RequestType Type;
    public readonly UrlPart[] Parts;
    public readonly string ReturnType;
    public readonly string Name;
    public readonly EndpointParameter[] Parameters;
    public readonly EndpointParameter? Body;

    public Endpoint(
        RequestType type,
        IEnumerable<UrlPart> parts,
        string returnType,
        string name,
        IEnumerable<EndpointParameter> parameters,
        EndpointParameter? body)
    {
        Type = type;
        
        if (IsPostOrPut() && body is null)
            throw new ArgumentException($"type is {type}, but body is null");
        
        Parts = parts.ToArray();
        ReturnType = returnType;
        Name = name;
        Parameters = parameters.ToArray();
        Body = body;
    }

    public bool IsPostOrPut()
    {
        return Type is RequestType.Post or RequestType.Put;
    }
}