using Analyzers;
using Microsoft.CodeAnalysis.Testing;

namespace CodeFixes.Tests;

public class UseBracesForSingleStatementsCodeFixProviderTests
{
    [Fact]
    public async Task IfStatement_WithoutBraces_ReportsDiagnostic()
    {
        const string source = """
                                class TestClass
                                {
                                    void TestMethod()
                                    {
                                        if (true)
                                            System.Console.WriteLine("test");
                                    }
                                }
                                """;

        const string fixedSource = """
                                 class TestClass
                                 {
                                     void TestMethod()
                                     {
                                         if (true)
                                         {
                                             System.Console.WriteLine("test");
                                         }
                                     }
                                 }
                                 """;

        var expected = DiagnosticResult
            .CompilerWarning(UseBracesForSingleStatementsAnalyzer.DiagnosticId)
            .WithLocation(6, 13);

        await source.VerifyCodeFixAsync<UseBracesForSingleStatementsAnalyzer, UseBracesForSingleStatementsCodeFixProvider>(fixedSource, expected);
    }
    
    [Fact]
    public async Task ForStatement_WithoutBraces_ReportsDiagnostic()
    {
        const string source = """
                                class TestClass
                                {
                                    void TestMethod()
                                    {
                                        for (int i = 0; i < 10; i++)
                                            System.Console.WriteLine(i);
                                    }
                                }
                                """;

        const string fixedSource = """
                                   class TestClass
                                   {
                                       void TestMethod()
                                       {
                                           for (int i = 0; i < 10; i++)
                                           {
                                               System.Console.WriteLine(i);
                                           }
                                       }
                                   }
                                   """;

        var expected = DiagnosticResult
            .CompilerWarning(UseBracesForSingleStatementsAnalyzer.DiagnosticId)
            .WithLocation(6, 13);

        await source.VerifyCodeFixAsync<UseBracesForSingleStatementsAnalyzer, UseBracesForSingleStatementsCodeFixProvider>(fixedSource, expected);
    }
    
    [Fact]
    public async Task ForEachStatement_WithoutBraces_ReportsDiagnostic()
    {
        const string source = """
                                using System.Collections.Generic;

                                class TestClass
                                {
                                    void TestMethod()
                                    {
                                        var list = new List<int> { 1, 2, 3 };
                                        foreach (var item in list)
                                            System.Console.WriteLine(item);
                                    }
                                }
                                """;

        const string fixedSource = """
                                 using System.Collections.Generic;

                                 class TestClass
                                 {
                                     void TestMethod()
                                     {
                                         var list = new List<int> { 1, 2, 3 };
                                         foreach (var item in list)
                                         {
                                             System.Console.WriteLine(item);
                                         }
                                     }
                                 }
                                 """;

        var expected = DiagnosticResult
            .CompilerWarning(UseBracesForSingleStatementsAnalyzer.DiagnosticId)
            .WithLocation(9, 13);

        await source.VerifyCodeFixAsync<UseBracesForSingleStatementsAnalyzer, UseBracesForSingleStatementsCodeFixProvider>(fixedSource, expected);
    }
    
    [Fact]
    public async Task WhileStatement_WithoutBraces_ReportsDiagnostic()
    {
        const string source = """
                              class TestClass
                              {
                                  void TestMethod()
                                  {
                                      int i = 0;
                                      while (i < 10)
                                          i++;
                                  }
                              }
                              """;

        const string fixedSource = """
                                   class TestClass
                                   {
                                       void TestMethod()
                                       {
                                           int i = 0;
                                           while (i < 10)
                                           {
                                               i++;
                                           }
                                       }
                                   }
                                   """;

        var expected = DiagnosticResult
            .CompilerWarning(UseBracesForSingleStatementsAnalyzer.DiagnosticId)
            .WithLocation(7, 13);

        await source.VerifyCodeFixAsync<UseBracesForSingleStatementsAnalyzer, UseBracesForSingleStatementsCodeFixProvider>(fixedSource, expected);
    }
    
    [Fact]
    public async Task DoStatement_WithoutBraces_ReportsDiagnostic()
    {
        const string source = """
                                class TestClass
                                {
                                    void TestMethod()
                                    {
                                        int i = 0;
                                        do
                                            i++;
                                        while (i < 10);
                                    }
                                }
                                """;

        const string fixedSource = """
                                 class TestClass
                                 {
                                     void TestMethod()
                                     {
                                         int i = 0;
                                         do
                                         {
                                             i++;
                                         }
                                         while (i < 10);
                                     }
                                 }
                                 """;

        var expected = DiagnosticResult
            .CompilerWarning(UseBracesForSingleStatementsAnalyzer.DiagnosticId)
            .WithLocation(7, 13);

        await source.VerifyCodeFixAsync<UseBracesForSingleStatementsAnalyzer, UseBracesForSingleStatementsCodeFixProvider>(fixedSource, expected);
    }

    [Fact]
    public async Task UsingStatement_WithoutBraces_ReportsDiagnostic()
    {
        const string source = """
                                using System.IO;

                                class TestClass
                                {
                                    void TestMethod()
                                    {
                                        using (var stream = new MemoryStream())
                                            stream.WriteByte(1);
                                    }
                                }
                                """;

        const string fixedSource = """
                                 using System.IO;

                                 class TestClass
                                 {
                                     void TestMethod()
                                     {
                                         using (var stream = new MemoryStream())
                                         {
                                             stream.WriteByte(1);
                                         }
                                     }
                                 }
                                 """;

        var expected = DiagnosticResult
            .CompilerWarning(UseBracesForSingleStatementsAnalyzer.DiagnosticId)
            .WithLocation(8, 13);

        await source.VerifyCodeFixAsync<UseBracesForSingleStatementsAnalyzer, UseBracesForSingleStatementsCodeFixProvider>(fixedSource, expected);
    }

    [Fact]
    public async Task LockStatement_WithoutBraces_ReportsDiagnostic()
    {
        const string source = """
                                class TestClass
                                {
                                    private readonly object _lock = new object();
                                    
                                    void TestMethod()
                                    {
                                        lock (_lock)
                                            System.Console.WriteLine("test");
                                    }
                                }
                                """;

        const string fixedSource = """
                                 class TestClass
                                 {
                                     private readonly object _lock = new object();
                                     
                                     void TestMethod()
                                     {
                                         lock (_lock)
                                         {
                                             System.Console.WriteLine("test");
                                         }
                                     }
                                 }
                                 """;

        var expected = DiagnosticResult
            .CompilerWarning(UseBracesForSingleStatementsAnalyzer.DiagnosticId)
            .WithLocation(8, 13);

        await source.VerifyCodeFixAsync<UseBracesForSingleStatementsAnalyzer, UseBracesForSingleStatementsCodeFixProvider>(fixedSource, expected);
    }
}
