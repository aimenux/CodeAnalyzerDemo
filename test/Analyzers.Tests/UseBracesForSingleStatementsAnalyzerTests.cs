using Microsoft.CodeAnalysis.Testing;

namespace Analyzers.Tests;

public class UseBracesForSingleStatementsAnalyzerTests
{
    [Theory]
    [InlineData("if (x > 0) x++;", 13, 20)]
    [InlineData("for (int i = 0; i < 10; i++) i++;", 13, 38)]
    [InlineData("foreach (var item in items) count++;", 13, 37)]
    [InlineData("foreach((var x, var y) in new[] { (1, 2) }) count++;", 13, 53)]
    [InlineData("while (x > 0) x--;", 13, 23)]
    [InlineData("do x--; while (x > 0);", 13, 12)]
    [InlineData("using (var file = File.OpenRead(path)) file.Read(buffer, 0, 100);", 13, 48)]
    [InlineData("lock (lockObject) count++;", 13, 27)]
    public Task Statements_WithoutBraces_ReportsDiagnostic(string statement, int expectedLine, int expectedColumn)
    {
        var source = $$"""
                        using System.IO;
                        class TestClass 
                        {
                            private readonly object lockObject = new object();
                            private int x;
                            private int count;
                            private int[] items = new int[10];
                            private string path = "test.txt";
                            private byte[] buffer = new byte[100];

                            void TestMethod() 
                            {
                                {{statement}}
                            }
                        }
                        """;
        
        var expected = DiagnosticResult
            .CompilerWarning(UseBracesForSingleStatementsAnalyzer.DiagnosticId)
            .WithLocation(expectedLine, expectedColumn);

        return source.VerifyAnalyzerAsync<UseBracesForSingleStatementsAnalyzer>(expected);
    }
}
