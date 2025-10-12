using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

namespace CodeFixes.Tests;

internal static class Extensions
{
    private static readonly IEnumerable<ReferenceAssemblies> DefaultFrameworks = [ReferenceAssemblies.Net.Net90];
    
    public static Task VerifyCodeFixAsync<TAnalyzer, TCodeFixProvider>(
        this string source,
        string fixedSource,
        params DiagnosticResult[] expected)
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFixProvider : CodeFixProvider, new()
    {
        return source.VerifyCodeFixAsync<TAnalyzer, TCodeFixProvider>(fixedSource, DefaultFrameworks, expected);
    }
    
    private static async Task VerifyCodeFixAsync<TAnalyzer, TCodeFixProvider>(
        this string source,
        string fixedSource,
        IEnumerable<ReferenceAssemblies> frameworks,
        params DiagnosticResult[] expected)
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFixProvider : CodeFixProvider, new()
    {
        foreach (var framework in frameworks)
        {
            var test = new CSharpCodeFixTest<TAnalyzer, TCodeFixProvider, DefaultVerifier>
            {
                TestCode = source,
                FixedCode = fixedSource,
                ReferenceAssemblies = framework
            };
        
            test.ExpectedDiagnostics.AddRange(expected);

            await test.RunAsync();
        }
    }
}