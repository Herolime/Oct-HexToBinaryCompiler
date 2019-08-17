namespace OctAndHexToBinaryCompiler.CodeAnalysis.SyntaxAnalysis
{
    public enum SyntaxKind
    {
     LetterToken,
     NumberToken,
     OctToken,
     HexToken,
     OctKeywordToken,
     HexKeywordToken,
     BadToken,
     WhiteSpaceToken,
     EndOfFileToken,
    LiteralExpression,
    PlusToken,
    MinusToken,
    StarToken,
    BinaryExpression
    }
}
