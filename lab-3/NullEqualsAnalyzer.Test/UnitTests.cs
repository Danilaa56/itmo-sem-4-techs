using AnalyzerTest;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NullEqualsAnalyzer.Test;

[TestClass]
public class CodeFixProviderTest : AnalyzerCodeFixProviderTestBase
{
    private const string Root = @"..\..\..\Data";

    [TestMethod]
    public async Task NullEqualsEqualsAnalyzerTest()
    {
        var code = ReadDataFile("Input1.cs");

        var diagnostics = await GetActualDiagnostics(code);

        Assert.AreEqual(1, diagnostics.Length);
    }

    [TestMethod]
    public async Task NullEqualsEqualsCodeFixTest()
    {
        var inputCode = ReadDataFile("Input1.cs");
        var expectedCode = ReadDataFile("Output1.cs");

        var outputCode = await CodeFix(inputCode);

        Assert.AreEqual(expectedCode, outputCode);
    }

    [TestMethod]
    public async Task NullNotEqualsCodeFixTest()
    {
        var inputCode = ReadDataFile("Input2.cs");
        var expectedCode = ReadDataFile("Output2.cs");

        var outputCode = await CodeFix(inputCode);

        Assert.AreEqual(expectedCode, outputCode);
    }

    protected override DiagnosticAnalyzer CreateAnalyzer() => new NullEqualsAnalyzer();
    protected override CodeFixProvider CreateCodeFixProvider() => new NullEqualsCodeFixProvider();

    private static string ReadDataFile(string name) => File.ReadAllText(Root + Path.DirectorySeparatorChar + name);

}