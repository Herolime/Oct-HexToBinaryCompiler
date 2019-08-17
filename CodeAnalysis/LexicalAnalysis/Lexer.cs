using OctAndHexToBinaryCompiler.CodeAnalysis.Diagnostics;
using OctAndHexToBinaryCompiler.CodeAnalysis.SyntaxAnalysis;

namespace OctAndHexToBinaryCompiler.CodeAnalysis.LexicalAnalysis
{
    internal sealed class Lexer
    {
        private readonly string _text;
        private int _position;
        private DiagnosticBag _diagnostics = new DiagnosticBag();
        public Lexer(string Text)
        {
            _text = Text;
        }
        public char Current => Peek(0);

        private char LookAhead => Peek(1);

        private char Peek (int offset)
        {
            var index = _position + offset;
            if (index >= _text.Length)
            {
                return '\0';
            }
            return _text[index];
        }

        public DiagnosticBag Diagnostics => _diagnostics;

        private void Next() 
        {
            _position++;
        }

        public SyntaxToken Lex()
        {

            if (_position >= _text.Length)
            {
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
            }

            if (char.IsDigit(Current))
            {
                var start = _position;
                var isHex = false;
                while (char.IsDigit(Current)) Next();
                if (char.IsUpper(Current))
                    {
                        isHex = true;
                        while (char.IsUpper(Current)) Next();   
                    }
                var length = _position - start;
                var text = _text.Substring(start, length);
                if (isHex == false)
                {
                    if (!int.TryParse(text, out var value))
                    {
                        _diagnostics.ReportInvalidNumber(text, typeof(int));
                    }   
                }

                return isHex || 
                    text.Contains("8") ||
                    text.Contains("9")? 
                    new SyntaxToken(SyntaxKind.HexToken, start, text, text)
                    :
                    new SyntaxToken(SyntaxKind.OctToken, start, text, text);
            }

            if (char.IsLetter(Current))
            {
                var start = _position;
                if (char.IsUpper(Current))
                {
                    while (char.IsUpper(Current)) Next();
                    if (char.IsDigit(Current))
                        while(char.IsDigit(Current)) Next();
                    var length = _position - start;
                    var text = _text.Substring(start, length);
                    // var kind = SyntaxFacts.GetKeywordKind(text);
                    return new SyntaxToken(SyntaxKind.HexToken, start, text, text);
                }
            }

            if (char.IsWhiteSpace(Current))
            {
                var start = _position;
                while (char.IsWhiteSpace(Current)) Next();
                var length = _position - start;
                var text = _text.Substring(start, length);
                return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, text, null);
            }

            switch (Current)
            {
                case '+':
                    return new SyntaxToken(SyntaxKind.PlusToken, _position++,"+", null);
                case '-':
                    return new SyntaxToken(SyntaxKind.MinusToken, _position++,"-", null);
                case '*':
                    return new SyntaxToken(SyntaxKind.StarToken, _position++,"+", null);
            //     case '/':
            //         return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
            //     case '(':
            //         return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
            //     case ')':
            //         return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);
            //     case '!':
            //         if(LookAhead == '=') return new SyntaxToken(SyntaxKind.BangEqualsToken, _position +=2, "!=", null);
            //         else return new SyntaxToken(SyntaxKind.BangToken, _position++, "!", null);
            //     case '&':
            //         if (LookAhead == '&') return new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, _position +=2, "&&", null);
            //         break;
            //     case '|':
            //         if (LookAhead == '|') return new SyntaxToken(SyntaxKind.PipePipeToken, _position +=2, "||", null);
            //         break;
            //     case '=':
            //         if (LookAhead == '=') return new SyntaxToken(SyntaxKind.EqualsEqualsToken, _position +=2, "==", null);
            //         break;
            }
            _diagnostics.ReportBadCharacter(_position, Current);
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}
