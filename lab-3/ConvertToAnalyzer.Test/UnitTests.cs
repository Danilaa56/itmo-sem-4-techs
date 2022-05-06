using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AnalyzerTest;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConvertToAnalyzer.Test;

[TestClass]
public class ConvertToCodeFixProviderTest : AnalyzerCodeFixProviderTestBase
{
    private const string Root = @"..\..\..\Data";

    [TestMethod]
    public async Task AnalyzerTest()
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
    public async Task ConvertIntCodeFixTest()
    {
        var inputCode = ReadDataFile("Input1.cs");
        var expectedCode = ReadDataFile("Output1.cs");

        var outputCode = await CodeFix(inputCode);

        Assert.AreEqual(expectedCode, outputCode);
    }
    
    [TestMethod]
    public async Task TestAllTypes()
    {
        var methodNames = "Boolean, Char, SByte, Byte, Int16, UInt16, Int32, UInt32, Int64, UInt64, Single, Double, Decimal, DateTime"
            .Split(", ").Select(type => "To" + type).ToArray();

        var typeNames = "bool, char, sbyte, byte, short, ushort, int, uint, long, ulong, float, double, decimal, DateTime"
            .Split(", ");
        
        var inputCode = ReadDataFile("Input1.cs");
        var expectedCode = ReadDataFile("Output1.cs");

        for (var i = 0; i < methodNames.Length; i++)
        {
            var outputCode = await CodeFix(inputCode.Replace("ToInt32", methodNames[i]));

            Assert.AreEqual(expectedCode.Replace("int.Parse", $"{typeNames[i]}.Parse"), outputCode);            
        }
    }

    protected override DiagnosticAnalyzer CreateAnalyzer() => new ConvertToAnalyzer();
    protected override CodeFixProvider CreateCodeFixProvider() => new ConvertToCodeFixProvider();
    
    private static string ReadDataFile(string name) => File.ReadAllText(Root + Path.DirectorySeparatorChar + name);
}