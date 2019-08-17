using System;

namespace OctAndHexToBinaryCompiler.CodeInterpretation.Binding
{
    internal abstract class BoundExpression : BoundNode
    {
        public abstract Type Type { get; }
    }
}