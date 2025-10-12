using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(UseBracesForSingleStatementsCodeFixProvider)), Shared]
    public sealed class UseBracesForSingleStatementsCodeFixProvider : CodeFixProvider
    {
        private const string Title = "Add braces for single statement block";
        
        public override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(UseBracesForSingleStatementsAnalyzer.DiagnosticId);
        
        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;
        
        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root is null) return;
            
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            
            var statement = root.FindToken(diagnosticSpan.Start)
                .Parent?
                .AncestorsAndSelf()
                .OfType<StatementSyntax>()
                .FirstOrDefault(s => s.Span == diagnosticSpan);

            if (statement is null || statement is BlockSyntax) return;

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: Title,
                    createChangedDocument: _ => Task.FromResult(AddBraces(context.Document, root, statement)),
                    equivalenceKey: Title),
                diagnostic);
        }
        
        private static Document AddBraces(Document document, SyntaxNode root, StatementSyntax statement)
        {
            var block = SyntaxFactory.Block(statement)
                .WithLeadingTrivia(statement.GetLeadingTrivia())
                .WithTrailingTrivia(statement.GetTrailingTrivia());
            
            var newRoot = root.ReplaceNode(statement, block);
            
            return document.WithSyntaxRoot(newRoot);
        }
    }
}