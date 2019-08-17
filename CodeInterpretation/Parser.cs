using OctAndHexToBinaryCompiler.CodeAnalysis.Diagnostics;
using OctAndHexToBinaryCompiler.CodeAnalysis.Expressions;
using OctAndHexToBinaryCompiler.CodeAnalysis.LexicalAnalysis;
using OctAndHexToBinaryCompiler.CodeAnalysis.SyntaxAnalysis;

namespace OctAndHexToBinaryCompiler.CodeInterpretation
{
    internal sealed class Parser
    {
        private readonly SyntaxToken[] _tokens;
       private int _position;
       private DiagnosticBag _diagnostics = new DiagnosticBag();

       public Parser(string text)
       {
           var tokens = new System.Collections.Generic.List<SyntaxToken>();
           var lexer = new Lexer(text);
           SyntaxToken token;
           do
           {
               token = lexer.Lex();
               if (token.Kind != SyntaxKind.WhiteSpaceToken &&
                    token.Kind != SyntaxKind.BadToken)
               {
                   tokens.Add(token);
               }
           } while (token.Kind != SyntaxKind.EndOfFileToken);

           _diagnostics.AddRange(lexer.Diagnostics);
           _tokens = tokens.ToArray();

       }

       private SyntaxToken MatchToken(SyntaxKind kind)
       {
           if (Current.Kind == kind) return NextToken(); 
           else 
           {
               _diagnostics.ReportUnexpectedToken(Current.Kind, kind);
               return new SyntaxToken(kind, Current.Position, null, null);
           }
       }

       public SyntaxTree Parse() 
        {
            var expression = ParseExpression();
            // var expression = ParsePrimaryExpression();
            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
            return new SyntaxTree(_diagnostics, expression, endOfFileToken);
        }

        private ExpressionSyntax ParseExpression(int parentPrecendence = 0)
       {
           ExpressionSyntax left = ParsePrimaryExpression(); 
           while (true)
           {
               var precedence = Current.Kind.GetBinaryOperatorPrecedence();
               if (precedence == 0 || precedence <= parentPrecendence) break;
               var operatorToken = NextToken();
               var right = ParseExpression(precedence);
               left = new BinaryExpressionSyntax(left, operatorToken, right);

           }

           return left;

       }

       private SyntaxToken NextToken()
        {

            // <numbers>
            // + - * / ( )
            // <whitespace>

            var current = Current;
            _position++;
            return current;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            switch (Current.Kind)
            {
                // case SyntaxKind.OpenParenthesisToken:
                //     {
                //         var left = NextToken();
                //         var expression = ParseExpression();
                //         var right = MatchToken(SyntaxKind.CloseParenthesisToken);
                //         return new ParenthesizedExpressionSyntax(left, expression, right);
                //     }

                // case SyntaxKind.FalseKeywordToken:
                // case SyntaxKind.TrueKeywordToken:
                //     {
                //         var keywordToken = NextToken();
                //         var value = keywordToken.Kind == SyntaxKind.TrueKeywordToken;
                //         return new LiteralExpressionSyntax(keywordToken, value);
                //     }
                case SyntaxKind.HexToken:
                    {
                        var token = MatchToken(SyntaxKind.HexToken);
                        return new LiteralExpressionSyntax(token);
                    }
                case SyntaxKind.OctToken:
                    {
                        var token = MatchToken(SyntaxKind.OctToken);
                        return new LiteralExpressionSyntax(token);
                    }
                default:
                {
                    var token = MatchToken(SyntaxKind.NumberToken);
                    return new LiteralExpressionSyntax(token);
                }
            }
        }

        private SyntaxToken Peek(int offset)
        {
            int index = _position + offset;
            if (index >= _tokens.Length)  return _tokens[_tokens.Length -1];
            return _tokens[index];
        }

       private SyntaxToken Current => Peek(0);

       public DiagnosticBag Diagnostics => _diagnostics;

    }
}