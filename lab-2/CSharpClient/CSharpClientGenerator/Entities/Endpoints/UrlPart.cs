namespace CSharpClientGenerator.Entities.Endpoints;

public sealed class UrlPart
{
    public readonly UrlPartType Type;
    public readonly string Str;

    public UrlPart(UrlPartType type, string str)
    {
        Type = type;
        Str = str;
    }

    public override string ToString()
    {
        return Type switch
        {
            UrlPartType.Var => Str,
            UrlPartType.Literal => '"' + Str + '"',
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}