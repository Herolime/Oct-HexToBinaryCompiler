using System.Collections.Generic;
using System.Linq;
using OctAndHexToBinaryCompiler.CodeAnalysis.Diagnostics;
using OctAndHexToBinaryCompiler.CodeAnalysis.Expressions;
using OctAndHexToBinaryCompiler.CodeInterpretation;

namespace OctAndHexToBinaryCompiler.CodeAnalysis.SyntaxAnalysis
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(IEnumerable<Diagnostic> _diagnostics, ExpressionSyntax root, SyntaxToken endOfFileToken)
        {
            Diagnostics = _diagnostics.ToArray();
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public IReadOnlyList<Diagnostic> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        public static SyntaxTree Parse(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }
    }
}