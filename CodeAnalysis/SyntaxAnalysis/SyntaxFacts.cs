namespace OctAndHexToBinaryCompiler.CodeAnalysis.SyntaxAnalysis
{
internal static class SyntaxFacts
    {
        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 5;
                case SyntaxKind.StarToken:
                    return 4;
                default:
                    return 0;
            }
        }
    }
}