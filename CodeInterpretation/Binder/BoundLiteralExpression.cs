using System;
using OctAndHexToBinaryCompiler.CodeAnalysis.SyntaxAnalysis;

namespace OctAndHexToBinaryCompiler.CodeInterpretation.Binding
{
    internal sealed class BoundLiteralExpression : BoundExpression
    {
        public BoundLiteralExpression(object value, SyntaxKind syntaxKind)
        {
            Value = value;
            SyntaxKind = syntaxKind;
        }

        public object Value { get; }

        public override Type Type => Value.GetType();

        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;

        public SyntaxKind SyntaxKind { get; }
    }
}