using System;
using System.Text;
using System.Text.RegularExpressions;
using dotObfs.Core.FSharp;

namespace dotObfs.Core.FSharp
{
    public class FSharpLexer
    {
        string source;
        int index;
        char current;

        public FSharpLexer(string source)
        {
            this.source = source;
            this.index = 0;
            this.current = source[index];
        }

        public void Advance()
        {
            index++;
            if (index >= source.Length)
                current = '\0';
            else
                current = source[index];
        }

        private void SkipWS()
        {
            while (char.IsWhiteSpace(current))
                Advance();
        }

        private void AppendAndAdvance(ref StringBuilder sb, char with)
        {
            sb.Append(with);
            Advance();
        }

        private void AppendAndAdvance(ref StringBuilder sb, string with)
        {
            sb.Append(with);
            Advance();
        }

        private Token ReadString()
        {
            StringBuilder sb = new StringBuilder();
            Advance();
            while (current != '"' && current != '\0')
                AppendAndAdvance(ref sb, current);
            if (current == '"')
                Advance();
            else
                throw new LexerException("Unterminated string literal");
            
            return new Token(
                TokenType.Text,
                Regex.Unescape(sb.ToString())
            );
        }

        private Token ReadRawStringLiteral()
        {
            StringBuilder sb = new StringBuilder();
            Advance();
            short doubleQuoteCount = default(short);
            bool success = true;

            while (current == '"')
            {
                doubleQuoteCount++;
                Advance();
            }
            
            if (current == '"')
                Advance();
            
            while (current != '\0')
            {
                if (current == '"')
                {
                    short dqCount = 0;
                    while (current == '"')
                    {
                        dqCount++;
                        AppendAndAdvance(ref sb, current);
                    }
                    
                    if (dqCount == doubleQuoteCount)
                    {
                        success = true;
                        break;
                    }
                }

                AppendAndAdvance(ref sb, current);
            }

            if (!success)
                throw new LexerException($"EOF never met (unterminated raw string literal)");

            if (current == '"')
                Advance();

            return new Token(
                TokenType.Text,
                Regex.Unescape(sb.ToString())
            );
        }

        private Token ReadChar()
        {
            char ch;
            StringBuilder temp = new StringBuilder();
            Advance();
            if (current == '\\')
            {
                Advance();
                temp.Append($"\\{current}");
                ch = Convert.ToChar(Regex.Unescape(temp.ToString()));
            }
            else
            {
                ch = current;
            }

            if (current == '\'')
                Advance();
            else
                throw new LexerException("Unterminated string literal");
            
            return new Token(
                TokenType.Character,
                ch.ToString()
            );
        }

        private Token ReadNumber()
        {
            StringBuilder num = new StringBuilder();
            Advance();
            while (char.IsDigit(current) ||
                   current == '_' ||
                   current == 'x' ||
                   current == 'X' ||
                   current == 'o' ||
                   current == 'O' ||
                   current == 'a' ||
                   current == 'A' ||
                   current == 'b' ||
                   current == 'B' ||
                   current == 'c' ||
                   current == 'C' ||
                   current == 'd' ||
                   current == 'D' ||
                   current == 'e' ||
                   current == 'E' ||
                   current == 'f' ||
                   current == 'F')
                AppendAndAdvance(ref num, current);
            
            return new Token(
                TokenType.Number,
                num.ToString()
                   .Replace("_", string.Empty)
            );
        }

        private Token ReadFloatingPoint()
        {
            StringBuilder num = new StringBuilder();
            Advance();
            while (char.IsDigit(current) ||
                   current == 'e' ||
                   current == 'E' ||
                   current == 'm' ||
                   current == 'M' ||
                   current == 'f' ||
                   current == 'F' ||
                   current == 'd' ||
                   current == 'D' ||
                   current == '.' ||
                   current == '_' ||
                   current == '-')
                AppendAndAdvance(ref num, current);

            return new Token(
                TokenType.Number,
                num.ToString()
                   .Replace("_", string.Empty)
            );
        }

        private Token ReadId()
        {
            StringBuilder id = new StringBuilder();
            while (char.IsLetterOrDigit(current) ||
                   current == '_')
                AppendAndAdvance(ref id, current);
            
            string idString = id.ToString();
            switch (idString)
            {
                                case "abstract":
                    return new Token(TokenType.Abstract);
                case "and":
                    return new Token(TokenType.And);
                case "as":
                    return new Token(TokenType.As);
                case "assert":
                    return new Token(TokenType.Assert);
                case "base":
                    return new Token(TokenType.Base);
                case "begin":
                    return new Token(TokenType.Begin);
                case "class":
                    return new Token(TokenType.Class);
                case "default":
                    return new Token(TokenType.Default);
                case "delegate":
                    return new Token(TokenType.Delegate);
                case "do":
                    return new Token(TokenType.Do);
                case "done":
                    return new Token(TokenType.Done);
                case "downcast":
                    return new Token(TokenType.Downcast);
                case "downto":
                    return new Token(TokenType.Downto);
                case "elif":
                    return new Token(TokenType.Elif);
                case "else":
                    return new Token(TokenType.Else);
                case "end":
                    return new Token(TokenType.End);
                case "exception":
                    return new Token(TokenType.Exception);
                case "extern":
                    return new Token(TokenType.Extern);
                case "false":
                    return new Token(TokenType.False);
                case "finally":
                    return new Token(TokenType.Finally);
                case "fixed":
                    return new Token(TokenType.Fixed);
                case "for":
                    return new Token(TokenType.For);
                case "fun":
                    return new Token(TokenType.Fun);
                case "function":
                    return new Token(TokenType.Function);
                case "if":
                    return new Token(TokenType.If);
                case "in":
                    return new Token(TokenType.In);
                case "inherit":
                    return new Token(TokenType.Inherit);
                case "inline":
                    return new Token(TokenType.Inline);
                case "interface":
                    return new Token(TokenType.Interface);
                case "internal":
                    return new Token(TokenType.Internal);
                case "lazy":
                    return new Token(TokenType.Lazy);
                case "let":
                    return new Token(TokenType.Let);
                case "match":
                    return new Token(TokenType.Match);
                case "member":
                    return new Token(TokenType.Member);
                case "module":
                    return new Token(TokenType.Module);
                case "mutable":
                    return new Token(TokenType.Mutable);
                case "namespace":
                    return new Token(TokenType.Namespace);
                case "new":
                    return new Token(TokenType.New);
                case "not":
                    return new Token(TokenType.Not);
                case "null":
                    return new Token(TokenType.Null);
                case "of":
                    return new Token(TokenType.Of);
                case "open":
                    return new Token(TokenType.Open);
                case "or":
                    return new Token(TokenType.Or);
                case "override":
                    return new Token(TokenType.Override);
                case "private":
                    return new Token(TokenType.Private);
                case "public":
                    return new Token(TokenType.Public);
                case "rec":
                    return new Token(TokenType.Rec);
                case "return":
                    return new Token(TokenType.Return);
                case "sig":
                    return new Token(TokenType.Sig);
                case "static":
                    return new Token(TokenType.Static);
                case "struct":
                    return new Token(TokenType.Struct);
                case "then":
                    return new Token(TokenType.Then);
                case "to":
                    return new Token(TokenType.To);
                case "true":
                    return new Token(TokenType.True);
                case "try":
                    return new Token(TokenType.Try);
                case "type":
                    return new Token(TokenType.Type);
                case "upcast":
                    return new Token(TokenType.Upcast);
                case "use":
                    return new Token(TokenType.Use);
                case "val":
                    return new Token(TokenType.Val);
                case "void":
                    return new Token(TokenType.Void);
                case "when":
                    return new Token(TokenType.When);
                case "while":
                    return new Token(TokenType.While);
                default:
                    return new Token(TokenType.Identifier, idString);
            }
        }

        public Token GetNextToken()
        {
            SkipWS();
            if (char.IsDigit(current))
                return ReadFloatingPoint();
            if (char.IsLetter(current))
                return ReadId();
            if (current == '"')
                return ReadString();
            if (current == '\'')
                return ReadChar();
            switch (current)
            {
                case '\0':
                    Advance();
                    return new Token(TokenType.EOF);
                case '(':
                    Advance();
                    return new Token(TokenType.LeftParenthesis, "(");
                case ')':
                    Advance();
                    return new Token(TokenType.RightParenthesis, ")");
                case '+':
                    Advance();
                    return new Token(TokenType.Plus, "+");
                case '-':
                    Advance();
                    return new Token(TokenType.Minus, "-");
                case '*':
                    Advance();
                    return new Token(TokenType.Star, "*");
                case '/':
                    Advance();
                    return new Token(TokenType.Slash, "/");
                case '%':
                    Advance();
                    return new Token(TokenType.Percent, "%");
                case '=':
                    Advance();
                    return new Token(TokenType.Equal, "=");
                case '!':
                    Advance();
                    return new Token(TokenType.ExclamationMark, "!");
                case '&':
                    Advance();
                    return new Token(TokenType._And, "&");
                case '|':
                    Advance();
                    return new Token(TokenType.Pipe, "|");
                case '^':
                    Advance();
                    return new Token(TokenType.Xor, "^");
                case '<':
                    Advance();
                    return new Token(TokenType.OpenAngleBracket, "<");
                case '>':
                    Advance();
                    return new Token(TokenType.CloseAngleBracket, ">");
                case '@':
                    Advance();
                    return new Token(TokenType.At, "@");
                case '#':
                    Advance();
                    return new Token(TokenType.Hashtag, "#");
                case '[':
                    Advance();
                    return new Token(TokenType.OpenBracket, "[");
                case ']':
                    Advance();
                    return new Token(TokenType.CloseBracket, "]");
                case '{':
                    Advance();
                    return new Token(TokenType.OpenCurlyBracket, "{");
                case '}':
                    Advance();
                    return new Token(TokenType.CloseCurlyBracket, "}");
                case '`':
                    Advance();
                    return new Token(TokenType.Backtick, "`");
                case '\'':
                    Advance();
                    return new Token(TokenType.SingleQuote, "'");
                case '"':
                    Advance();
                    return new Token(TokenType.DoubleQuote, "\"");
                case '~':
                    Advance();
                    return new Token(TokenType.Tilde, "~");
                case ';':
                    Advance();
                    return new Token(TokenType.Semicolon, ";");
                case ':':
                    Advance();
                    return new Token(TokenType.Colon, ":");
                case ',':
                    Advance();
                    return new Token(TokenType.Comma, ",");
                case '.':
                    Advance();
                    return new Token(TokenType.Dot, ".");
                case '_':
                    Advance();
                    return new Token(TokenType.Underscore, "_");
                case '\\':
                    Advance();
                    return new Token(TokenType.Backslash, "\\");
                case '?':
                    Advance();
                    return new Token(TokenType.QuestionMark, "?");
                default:
                    throw new LexerException($"The token is invalid: {current}. Seeing this error is uncommon - usually it is skipped, but it is normal to see this in a log file.");
            }
        }
    }
}
