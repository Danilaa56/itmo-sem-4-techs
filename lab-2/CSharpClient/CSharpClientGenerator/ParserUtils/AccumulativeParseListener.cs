using CSharpClientGenerator.Entities;

namespace CSharpClientGenerator.ParserUtils;

public sealed class AccumulativeParseListener : JavaParserBaseListener
{
    private readonly List<ClassDeclaration> _classes = new();
    private readonly List<RecordDeclaration> _records = new();

    public override void ExitClassDeclaration(JavaParser.ClassDeclarationContext context)
    {
        _classes.Add(context.Parse());
    }

    public override void ExitRecordDeclaration(JavaParser.RecordDeclarationContext context)
    {
        _records.Add(context.Parse());
    }

    public List<ClassDeclaration> PopClasses() => Pop(_classes);
    public List<RecordDeclaration> PopRecords() => Pop(_records);

    private static List<T> Pop<T>(ICollection<T> collection)
    {
        var result = collection.ToList();
        collection.Clear();
        return result;
    }
}