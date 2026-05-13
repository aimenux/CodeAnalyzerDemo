using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class UseBracesForSingleStatementsAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "RULE001";
        private static readonly LocalizableString Title = "Use braces for single statement blocks";
        private static readonly LocalizableString MessageFormat = "Single statement blocks should use braces";
        private static readonly LocalizableString Description = "Single statement blocks should use braces for consistency.";
        private const DiagnosticSeverity Severity = DiagnosticSeverity.Warning;
        private const bool IsEnabledByDefault = true;
        private const string Category = "Style";

        private static readonly DiagnosticDescriptor Rule = new(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            Severity,
            IsEnabledByDefault,
            Description);
        
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

        private static readonly ImmutableDictionary<SyntaxKind, Func<SyntaxNode, StatementSyntax>> Extractors =
            new Dictionary<SyntaxKind, Func<SyntaxNode, StatementSyntax>>
            {
                [SyntaxKind.IfStatement] = node => ((IfStatementSyntax)node).Statement,
                [SyntaxKind.ForStatement] = node => ((ForStatementSyntax)node).Statement,
                [SyntaxKind.ForEachStatement] = node => ((ForEachStatementSyntax)node).Statement,
                [SyntaxKind.ForEachVariableStatement] = node => ((ForEachVariableStatementSyntax)node).Statement,
                [SyntaxKind.WhileStatement] = node => ((WhileStatementSyntax)node).Statement,
                [SyntaxKind.DoStatement] = node => ((DoStatementSyntax)node).Statement,
                [SyntaxKind.UsingStatement] = node => ((UsingStatementSyntax)node).Statement,
                [SyntaxKind.LockStatement] = node => ((LockStatementSyntax)node).Statement
            }.ToImmutableDictionary();

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(Analyze, Extractors.Keys.ToArray());
        }
        
        private static void Analyze(SyntaxNodeAnalysisContext context)
        {
            if (!Extractors.TryGetValue(context.Node.Kind(), out var extractor))
            {
                return;
            }

            var statement = extractor(context.Node);
            if (statement == null) return;
            if (statement.IsKind(SyntaxKind.Block)) return;
            if (statement.IsKind(SyntaxKind.EmptyStatement)) return;
            var diagnostic = Diagnostic.Create(Rule, statement.GetLocation());
            context.ReportDiagnostic(diagnostic);
        }
    }
}
