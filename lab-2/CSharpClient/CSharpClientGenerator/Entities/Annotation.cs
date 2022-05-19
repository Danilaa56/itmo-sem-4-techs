namespace CSharpClientGenerator.Entities;

public sealed class Annotation
{
    public readonly string Name;
    public readonly string? Value;

    public Annotation(string name, string? value)
    {
        Name = name;
        Value = value;
        if (value is not null)
        {
            Value = value.Substring(1, value.Length - 2);
        }
    }

    public override string ToString()
    {
        if (Value is null)
            return "@" + Name;
        return $"@{Name}(\"{Value}\")";
    }
}
