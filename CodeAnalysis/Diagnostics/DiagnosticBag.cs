using System;
using System.Collections;
using System.Collections.Generic;
using OctAndHexToBinaryCompiler.CodeAnalysis.SyntaxAnalysis;

namespace OctAndHexToBinaryCompiler.CodeAnalysis.Diagnostics
{
    internal sealed class DiagnosticBag : IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public void Report(string message)
        {
            _diagnostics.Add(new Diagnostic(message));
        }
        public IEnumerator<Diagnostic> GetEnumerator()
        {
            return _diagnostics.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void ReportInvalidNumber(string text, Type type)
        {
            var message = $"ERROR: The Number {text} isn´t a valid int32";
            Report(message);
        }

        public void AddRange(DiagnosticBag diagnostics)
        {
            _diagnostics.AddRange(diagnostics);
        }

        public void ReportBadCharacter(int position, char character)
        {
            var message = $"ERROR: bad character input '{character}'";
            // var span = new TextSpan(position, 1);
            Report(message);
        }

        public void ReportUnexpectedToken(SyntaxKind actualKind, SyntaxKind expectedKind)
        {
            var message = $"ERROR: Unexpected token <{actualKind}>, was expecting <{expectedKind}>";
            Report(message);
        }

        public void ReportUndefinedBinaryOperator(string operatorText, Type leftType, Type rightType)
        {
            var message = $"ERROR: Binary Operator {operatorText} is not defined for types {leftType} and {rightType}";
            Report(message);
        }

        public void ReportUndefinedUnaryOperator(string operatorText, Type operandType)
        {
            var message = $"ERROR: Unary Operator {operatorText} is not defined for type {operandType}";
            Report(message);
        }
    }
}