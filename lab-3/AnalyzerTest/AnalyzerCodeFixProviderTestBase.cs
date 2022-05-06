using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AnalyzerTest;

public abstract class AnalyzerCodeFixProviderTestBase
{
    protected abstract DiagnosticAnalyzer CreateAnalyzer();
    protected abstract CodeFixProvider CreateCodeFixProvider();

    protected class TestCompilationContext
    {
        public Workspace Workspace;
        public Solution Solution => Workspace.CurrentSolution;
        public Project Project => Solution.GetProject(ProjectId);
        public Document Document => Solution.GetDocument(DocumentId);
        public ProjectId ProjectId;
        public DocumentId DocumentId;
        public string Code;
        public ImmutableArray<Diagnostic> Diagnostics;
    }

    private TestCompilationContext CreateProjectWithCode(string code)
    {
        var workspace = new AdhocWorkspace();

        var solution = workspace.CurrentSolution;

        var projectId = ProjectId.CreateNewId();
        var documentId = DocumentId.CreateNewId(projectId);

        solution = solution
            .AddProject(
                projectId,
                "My Test Project",
                "MyTestProject",
                LanguageNames.CSharp)
            .AddDocument(
                documentId,
                "File.cs",
                code);

        var project = solution.GetProject(projectId);
        project = project!.AddMetadataReference(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));

        if (!workspace.TryApplyChanges(project.Solution))
        {
            throw new Exception("Unable to apply changes to the workspace");
        }

        return new TestCompilationContext
        {
            Workspace = workspace,
            ProjectId = projectId,
            DocumentId = documentId,
            Code = code,
        };
    }

    protected async Task<TestCompilationContext> Diagnose(string code)
    {
        var testCompilationContext = CreateProjectWithCode(code);

        var compilation = await testCompilationContext.Project.GetCompilationAsync();
        var compilationWithAnalyzer = compilation!.WithAnalyzers(ImmutableArray.Create(CreateAnalyzer()));

        testCompilationContext.Diagnostics = await compilationWithAnalyzer.GetAllDiagnosticsAsync();

        return testCompilationContext;
    }

    protected async Task<ImmutableArray<Diagnostic>> GetDiagnostics(string code)
    {
        var testContext = await Diagnose(code);
        return testContext.Diagnostics;
    }
    
    protected async Task<ImmutableArray<Diagnostic>> GetActualDiagnostics(string code)
    {
        var analyzerDiagnostics = CreateAnalyzer().SupportedDiagnostics.Select(d => d.Id);
        return (await GetDiagnostics(code))
            .Where(d => analyzerDiagnostics.Contains(d.Id)).ToImmutableArray();
    }
    
    protected async Task<string> CodeFix(string inputCode)
    {
        var testCompilationContext = await Diagnose(inputCode);

        var codeFixProvider = CreateCodeFixProvider();
        var fixableDiagnostics = codeFixProvider.FixableDiagnosticIds;

        var diagnostics = testCompilationContext.Diagnostics
            .Where(d => fixableDiagnostics.Contains(d.Id)).ToList();

        CodeAction registeredCodeAction = null;

        var context = new CodeFixContext(testCompilationContext.Document,
            diagnostics[0],
            (codeAction, _) => {
                registeredCodeAction = codeAction;
            },
            CancellationToken.None);

        await codeFixProvider.RegisterCodeFixesAsync(context);

        if (registeredCodeAction == null)
        {
            throw new Exception("Code action was not registered");
        }

        var operations = await registeredCodeAction.GetOperationsAsync(CancellationToken.None);

        foreach (var operation in operations)
        {
            operation.Apply(testCompilationContext.Workspace, CancellationToken.None);
        }

        var updatedCode = (await testCompilationContext.Document.GetTextAsync()).ToString();
        return updatedCode;
    }
}