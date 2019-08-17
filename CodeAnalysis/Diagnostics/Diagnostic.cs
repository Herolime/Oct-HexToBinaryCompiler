namespace OctAndHexToBinaryCompiler.CodeAnalysis.Diagnostics
{

    public sealed class Diagnostic
    {
        public Diagnostic (string message)
        {
            Message = message;
        }

        // public TextSpan Span { get; }
        public string Message { get; }

        public override string ToString() => Message;
    }
}
