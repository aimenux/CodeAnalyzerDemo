using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace Analyzers.Tests;

internal static class Extensions
{
    private static readonly IEnumerable<ReferenceAssemblies> DefaultFrameworks = [ReferenceAssemblies.Net.Net90];
    
    public static Task VerifyAnalyzerAsync<TAnalyzer>(
        this string source,
        params DiagnosticResult[] expected)
        where TAnalyzer : DiagnosticAnalyzer, new()
    {
        return source.VerifyAnalyzerAsync<TAnalyzer>(DefaultFrameworks, expected);
    }
    
    private static async Task VerifyAnalyzerAsync<TAnalyzer>(
        this string source,
        IEnumerable<ReferenceAssemblies> frameworks,
        params DiagnosticResult[] expected)
        where TAnalyzer : DiagnosticAnalyzer, new()
    {
        foreach (var framework in frameworks)
        {
            var test = new CSharpAnalyzerTest<TAnalyzer, DefaultVerifier>
            {
                TestCode = source,
                ReferenceAssemblies = framework
            };
        
            test.ExpectedDiagnostics.AddRange(expected);

            await test.RunAsync();
        }
    }
}