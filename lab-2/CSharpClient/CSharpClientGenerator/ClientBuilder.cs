using System.Collections.Immutable;
using Antlr4.Runtime;
using CSharpClientGenerator.Entities;
using CSharpClientGenerator.Extensions;
using CSharpClientGenerator.ParserUtils;

namespace CSharpClientGenerator;

public sealed class ClientBuilder
{
    private readonly HashSet<string> _stdTypes = new()
    {
        "bool", "char", "byte", "short",
        "int", "long", "float", "double",
        "string", "object", "void",
        "IEnumerable", "Guid", "DateTime"
    };

    private readonly char _separator = Path.DirectorySeparatorChar;
    
    public void GenerateFromJavaSource(string javaSrcDir, string destination, string package)
    {
        var dtoPackage = $"{package}.Dto";
        var dtoDestination = $@"{destination}{_separator}Dto";
        if (Directory.Exists(destination))
            Directory.Delete(destination, true);
        Directory.CreateDirectory(destination);
        Directory.CreateDirectory(dtoDestination);

        var listener = new AccumulativeParseListener();
        var renamer = new JavaToCSharpRenamer();

        var javaFiles = FindJavaFiles(javaSrcDir);
        javaFiles.ForEach(f => Parse(listener, f));

        var classes = listener.PopClasses().ToList();
        var records = listener.PopRecords().ToList();

        var typeLinks = classes.SelectMany(c => c.GetAllTypeLinks())
            .Union(records.SelectMany(r => r.GetAllTypeLinks()))
            .ToHashSet();

        var methods = classes.SelectMany(c => c.MethodDeclarations);

        renamer.RenameTypeLinks(typeLinks);
        renamer.RenameMethods(methods);

        var controllers = FindControllers(classes);
        var requiredTypes = FindRequiredRecords(controllers, records);
        
        foreach (var controller in controllers)
        {
            var shouldAddUsing = controller.GetAllTypeLinks().Select(type => type.Name)
                .Any(name => !_stdTypes.Contains(name));

            var usings = new List<string>();
            if (shouldAddUsing)
                usings.Add(dtoPackage);
            
            var code = controller.ToCSharpCode(package, usings);
            File.WriteAllText($@"{destination}{_separator}{controller.Name}.cs", code);
        }
        
        foreach (var requiredType in requiredTypes)
        {
            var code = requiredType.ToCSharpCode(dtoPackage);
            File.WriteAllText($@"{dtoDestination}{_separator}{requiredType.Name}.cs", code);
        }
    }

    private List<string> FindJavaFiles(string dirName)
    {
        var javaFiles = new List<string>();

        foreach (var dir in Directory.GetDirectories(dirName))
            javaFiles.AddRange(FindJavaFiles(dir));
        javaFiles.AddRange(Directory.GetFiles(dirName).Where(f => f.EndsWith(".java")));

        return javaFiles;
    }

    private IEnumerable<RecordDeclaration> FindRequiredRecords(
        IEnumerable<ClassDeclaration> controllers,
        ICollection<RecordDeclaration> records)
    {
        var requiredTypes = controllers
            .SelectMany(controller => controller.GetAllTypeLinks())
            .Select(type => type.Name)
            .Where(name => !_stdTypes.Contains(name)).ToHashSet();

        var recordsRequirements = new Dictionary<string, HashSet<string>>();
        foreach (var record in records)
        {
            recordsRequirements[record.Name] = record.GetAllTypeLinks()
                .Select(type => type.Name)
                .Where(name => !_stdTypes.Contains(name)).ToHashSet();
        }

        var addedTypes = new HashSet<string>();
        while (requiredTypes.Count > 0)
        {
            var requiredType = requiredTypes.First();

            if (!recordsRequirements.TryGetValue(requiredType, out var recordRequirements))
            {
                throw new Exception($"Record '{requiredType}' required but was not found");
            }
            
            addedTypes.Add(requiredType);
            requiredTypes.Remove(requiredType);
            
            foreach (var toRequire in recordRequirements.Where(req => !addedTypes.Contains(req)))
            {
                requiredTypes.Add(toRequire);
            }
        }

        return records.Where(record => addedTypes.Contains(record.Name));
    }

    private static void Parse(AccumulativeParseListener listener, string javaFileName)
    {
        var antlrFileStream = new AntlrFileStream(javaFileName);
        var lexer = new JavaLexer(antlrFileStream);
        var tokenStream = new CommonTokenStream(lexer);
        var parser = new JavaParser(tokenStream);

        parser.AddParseListener(listener);

        parser.compilationUnit();
    }

    private static ClassDeclaration[] FindControllers(IEnumerable<ClassDeclaration> classDeclarations)
    {
        var controllers = classDeclarations.Where(c => c.HasAnnotation("RestController")).ToArray();
        foreach (var classDeclaration in controllers)
        {
            classDeclaration.MethodDeclarations = classDeclaration.MethodDeclarations.Where(m =>
                m.HasAnnotation("GetMapping")
                || m.HasAnnotation("PutMapping")
                || m.HasAnnotation("DeleteMapping")
                || m.HasAnnotation("PostMapping")).ToImmutableArray();
        }

        return controllers;
    }
}