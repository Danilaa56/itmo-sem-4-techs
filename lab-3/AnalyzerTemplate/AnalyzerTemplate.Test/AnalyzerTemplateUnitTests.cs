using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS =AnalyzerTemplate.Test.CSharpCodeFixVerifier<
    AnalyzerTemplate.AnalyzerTemplateAnalyzer,
    AnalyzerTemplate.AnalyzerTemplateCodeFixProvider>;

namespace AnalyzerTemplate.Test
{
    // [TestClass]
    public class AnalyzerTemplateUnitTest
    {
        private const string ResourcesPath = "../../../Resources/";
        
        //No diagnostics expected to show up
        // [TestMethod]
        // public async Task TestMethod1()
        // {
        //     ReadTestData("Test1", out var input, out var output);
        //     // var test = @"";
        //
        //     await VerifyCS.VerifyAnalyzerAsync(input);
        // }
        
        // [DataTestMethod]
        // [DataRow("Test1")]
        // [DataRow("Test2")]
        // public async Task Test(string name)
        // {
        //     ReadTestData(name, out var input, out var output);
        //
        //     var expected = VerifyCS.Diagnostic("AnalyzerTemplate").WithLocation(0).WithArguments("TypeName");
        //     await VerifyCS.VerifyCodeFixAsync(input, expected, output);
        // }

        private static void ReadTestData(string name, out string input, out string output)
        {
            input = File.ReadAllText($"{ResourcesPath}/Input/{name}.cs");
            output = File.ReadAllText($"{ResourcesPath}/Output/{name}.cs");
        }
        // //Diagnostic and CodeFix both triggered and checked for
        // [TestMethod]
        // public async Task TestMethod2()
        // {
        //     var test = File.ReadAllText("../../../Resources/Input/Test2.cs");
        //     var fixtest = File.ReadAllText("../../../Resources/Output/Test2.cs");
        //     var expected = VerifyCS.Diagnostic("AnalyzerTemplate").WithLocation(0).WithArguments("TypeName");
        //     await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        // }
    }
}
