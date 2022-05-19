namespace CSharpClientGenerator.Entities.Endpoints;

public sealed class EndpointParameter
{
    public readonly string Type;
    public readonly string Name;

    public EndpointParameter(string type, string name)
    {
        Type = type;
        Name = name;
    }
}