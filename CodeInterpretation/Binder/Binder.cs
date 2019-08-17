using System;
using OctAndHexToBinaryCompiler.CodeAnalysis.Diagnostics;
using OctAndHexToBinaryCompiler.CodeAnalysis.Expressions;
using OctAndHexToBinaryCompiler.CodeAnalysis.SyntaxAnalysis;

namespace OctAndHexToBinaryCompiler.CodeInterpretation.Binding
{
    internal sealed class Binder 
    {
        private DiagnosticBag _diagnostics = new DiagnosticBag();
        public DiagnosticBag Diagnostics => _diagnostics; 
        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch (syntax.Kind)
            {
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax)syntax);
                // case SyntaxKind.UnaryExpression:
                //     return BindUnaryExpression((UnaryExpressionSyntax)syntax);
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpressionSyntax)syntax);
                // case SyntaxKind.ParenthesizedExpression:
                //     return BindExpression(((ParenthesizedExpressionSyntax)syntax).Expression);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }


        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);
            var boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);
            if (boundOperator == null)
            {
                _diagnostics.ReportUndefinedBinaryOperator(syntax.OperatorToken.Text, boundLeft.Type, boundRight.Type);
                return boundLeft;

            }
            return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);

        }

        // private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        // {
        //     var boundOperand = BindExpression(syntax.Operand);
        //     var boundOperator = BoundUnaryOperator.Bind(syntax.Operand.Kind, boundOperand.Type);
        //     if (boundOperator == null)
        //     {
        //         _diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundOperand.Type);
        //         return boundOperand;
        //     }
        //     else
        //     {
        //         return new BoundUnaryExpression(boundOperator, boundOperand);    
        //     }

        // }

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            var value = syntax.Value ?? 0;
            return new BoundLiteralExpression(value, syntax.LiteralToken.Kind);
        }
        
    }
}
