using System;
using System.IO;
using System.Threading.Tasks;
using AnalyzerTest;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EqualsEqualsEqualsAnalyzer.Test;

[TestClass]
public class EqualsEqualsEqualsCodeFixProviderTest : AnalyzerCodeFixProviderTestBase
{
    private const string Root = @"..\..\..\Data";

    [TestMethod]
    public async Task NotOverloadedOperatorAnalyzerTest()
    {
        var code = ReadDataFile("Input1.cs");

        var diagnostics = await GetActualDiagnostics(code);
        foreach (var diagnostic in diagnostics)
        {
            Console.WriteLine(diagnostic);
        }

        Assert.AreEqual(1, diagnostics.Length);
    }

    [TestMethod]
    public async Task OverloadedOperatorAnalyzerTest()
    {
        var code = ReadDataFile("Input2.cs");

        var diagnostics = await GetActualDiagnostics(code);
        foreach (var diagnostic in diagnostics)
        {
            Console.WriteLine(diagnostic);
        }

        Assert.AreEqual(0, diagnostics.Length);
    }
    
    [TestMethod]
    public async Task InheritsOperatorAnalyzerTest()
    {
        var code = ReadDataFile("Input3.cs");

        var diagnostics = await GetActualDiagnostics(code);
        foreach (var diagnostic in diagnostics)
        {
            Console.WriteLine(diagnostic);
        }

        Assert.AreEqual(0, diagnostics.Length);
    }
    
    [TestMethod]
    public async Task NotOverloadedOperatorCodeFixTest()
    {
        var inputCode = ReadDataFile("Input1.cs");
        var expectedCode = ReadDataFile("Output1.cs");

        var outputCode = await CodeFix(inputCode);

        Assert.AreEqual(expectedCode, outputCode);
    }

    protected override DiagnosticAnalyzer CreateAnalyzer() => new EqualsEqualsEqualsAnalyzer();
    protected override CodeFixProvider CreateCodeFixProvider() => new EqualsEqualsEqualsCodeFixProvider();
    
    private static string ReadDataFile(string name) => File.ReadAllText(Root + Path.DirectorySeparatorChar + name);
}