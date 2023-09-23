using System;
using System.Text;
using System.Text.RegularExpressions;
using dotObfs.Core.C;

namespace dotObfs.Core.C
{
    public class CLexer
    {
        string source;
        int index;
        char current;

        public CLexer(string source)
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
                case "auto":
                    return new Token(TokenType.Auto);
                case "break":
                    return new Token(TokenType.Break);
                case "case":
                    return new Token(TokenType.Case);
                case "char":
                    return new Token(TokenType.Char);
                case "const":
                    return new Token(TokenType.Const);
                case "continue":
                    return new Token(TokenType.Continue);
                case "default":
                    return new Token(TokenType.Default);
                case "do":
                    return new Token(TokenType.Do);
                case "double":
                    return new Token(TokenType.Double);
                case "else":
                    return new Token(TokenType.Else);
                case "enum":
                    return new Token(TokenType.Enum);
                case "extern":
                    return new Token(TokenType.Extern);
                case "float":
                    return new Token(TokenType.Float);
                case "for":
                    return new Token(TokenType.For);
                case "goto":
                    return new Token(TokenType.Goto);
                case "if":
                    return new Token(TokenType.If);
                case "int":
                    return new Token(TokenType.Int);
                case "long":
                    return new Token(TokenType.Long);
                case "register":
                    return new Token(TokenType.Register);
                case "return":
                    return new Token(TokenType.Return);
                case "short":
                    return new Token(TokenType.Short);
                case "signed":
                    return new Token(TokenType.Signed);
                case "sizeof":
                    return new Token(TokenType.Sizeof);
                case "static":
                    return new Token(TokenType.Static);
                case "struct":
                    return new Token(TokenType.Struct);
                case "switch":
                    return new Token(TokenType.Switch);
                case "typedef":
                    return new Token(TokenType.Typedef);
                case "union":
                    return new Token(TokenType.Union);
                case "unsigned":
                    return new Token(TokenType.Unsigned);
                case "void":
                    return new Token(TokenType.Void);
                case "volatile":
                    return new Token(TokenType.Volatile);
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
                    return new Token(TokenType.And, "&");
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
