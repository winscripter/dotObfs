using System;
using System.Text;
using System.Text.RegularExpressions;
using dotObfs.Core.Cpp;

namespace dotObfs.Core.Cpp
{
    public class CppLexer
    {
        string source;
        int index;
        char current;

        public CppLexer(string source)
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
                case "alignas":
                    return new Token(TokenType.Alignas);
                case "alignof":
                    return new Token(TokenType.Alignof);
                case "and":
                    return new Token(TokenType.And);
                case "and_eq":
                    return new Token(TokenType.And_eq);
                case "asm":
                    return new Token(TokenType.Asm);
                case "auto":
                    return new Token(TokenType.Auto);
                case "bitand":
                    return new Token(TokenType.Bitand);
                case "bitor":
                    return new Token(TokenType.Bitor);
                case "bool":
                    return new Token(TokenType.Bool);
                case "break":
                    return new Token(TokenType.Break);
                case "case":
                    return new Token(TokenType.Case);
                case "catch":
                    return new Token(TokenType.Catch);
                case "char":
                    return new Token(TokenType.Char);
                case "char8_t":
                    return new Token(TokenType.Char8_t);
                case "char16_t":
                    return new Token(TokenType.Char16_t);
                case "char32_t":
                    return new Token(TokenType.Char32_t);
                case "class":
                    return new Token(TokenType.Class);
                case "compl":
                    return new Token(TokenType.Compl);
                case "concept":
                    return new Token(TokenType.Concept);
                case "const":
                    return new Token(TokenType.Const);
                case "const_cast":
                    return new Token(TokenType.Const_cast);
                case "consteval":
                    return new Token(TokenType.Consteval);
                case "constexpr":
                    return new Token(TokenType.Constexpr);
                case "constinit":
                    return new Token(TokenType.Constinit);
                case "continue":
                    return new Token(TokenType.Continue);
                case "co_await":
                    return new Token(TokenType.Co_await);
                case "co_return":
                    return new Token(TokenType.Co_return);
                case "co_yield":
                    return new Token(TokenType.Co_yield);
                case "decltype":
                    return new Token(TokenType.Decltype);
                case "default":
                    return new Token(TokenType.Default);
                case "delete":
                    return new Token(TokenType.Delete);
                case "do":
                    return new Token(TokenType.Do);
                case "double":
                    return new Token(TokenType.Double);
                case "dynamic_cast":
                    return new Token(TokenType.Dynamic_cast);
                case "else":
                    return new Token(TokenType.Else);
                case "enum":
                    return new Token(TokenType.Enum);
                case "explicit":
                    return new Token(TokenType.Explicit);
                case "export":
                    return new Token(TokenType.Export);
                case "extern":
                    return new Token(TokenType.Extern);
                case "false":
                    return new Token(TokenType.False);
                case "float":
                    return new Token(TokenType.Float);
                case "for":
                    return new Token(TokenType.For);
                case "friend":
                    return new Token(TokenType.Friend);
                case "goto":
                    return new Token(TokenType.Goto);
                case "if":
                    return new Token(TokenType.If);
                case "inline":
                    return new Token(TokenType.Inline);
                case "int":
                    return new Token(TokenType.Int);
                case "long":
                    return new Token(TokenType.Long);
                case "mutable":
                    return new Token(TokenType.Mutable);
                case "namespace":
                    return new Token(TokenType.Namespace);
                case "new":
                    return new Token(TokenType.New);
                case "noexcept":
                    return new Token(TokenType.Noexcept);
                case "not":
                    return new Token(TokenType.Not);
                case "not_eq":
                    return new Token(TokenType.Not_eq);
                case "nullptr":
                    return new Token(TokenType.Nullptr);
                case "operator":
                    return new Token(TokenType.Operator);
                case "or":
                    return new Token(TokenType.Or);
                case "or_eq":
                    return new Token(TokenType.Or_eq);
                case "private":
                    return new Token(TokenType.Private);
                case "protected":
                    return new Token(TokenType.Protected);
                case "public":
                    return new Token(TokenType.Public);
                case "register":
                    return new Token(TokenType.Register);
                case "reinterpret_cast":
                    return new Token(TokenType.Reinterpret_cast);
                case "requires":
                    return new Token(TokenType.Requires);
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
                case "static_assert":
                    return new Token(TokenType.Static_assert);
                case "static_cast":
                    return new Token(TokenType.Static_cast);
                case "struct":
                    return new Token(TokenType.Struct);
                case "switch":
                    return new Token(TokenType.Switch);
                case "template":
                    return new Token(TokenType.Template);
                case "this":
                    return new Token(TokenType.This);
                case "thread_local":
                    return new Token(TokenType.Thread_local);
                case "throw":
                    return new Token(TokenType.Throw);
                case "true":
                    return new Token(TokenType.True);
                case "try":
                    return new Token(TokenType.Try);
                case "typedef":
                    return new Token(TokenType.Typedef);
                case "typeid":
                    return new Token(TokenType.Typeid);
                case "typename":
                    return new Token(TokenType.Typename);
                case "union":
                    return new Token(TokenType.Union);
                case "unsigned":
                    return new Token(TokenType.Unsigned);
                case "using":
                    return new Token(TokenType.Using);
                case "virtual":
                    return new Token(TokenType.Virtual);
                case "void":
                    return new Token(TokenType.Void);
                case "volatile":
                    return new Token(TokenType.Volatile);
                case "wchar_t":
                    return new Token(TokenType.Wchar_t);
                case "while":
                    return new Token(TokenType.While);
                case "xor":
                    return new Token(TokenType.Xor);
                case "xor_eq":
                    return new Token(TokenType.Xor_eq);
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
                    return new Token(TokenType._Xor, "^");
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
