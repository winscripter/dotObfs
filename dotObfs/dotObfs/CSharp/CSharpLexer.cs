using System;
using System.Text;
using System.Text.RegularExpressions;
using dotObfs.Core.CSharp;

namespace dotObfs.Core.CSharp
{
    public class CSharpLexer
    {
        string source;
        int index;
        char current;

        public CSharpLexer(string source)
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
                case "as":
                    return new Token(TokenType.As);
                case "base":
                    return new Token(TokenType.Base);
                case "bool":
                    return new Token(TokenType.Bool);
                case "break":
                    return new Token(TokenType.Break);
                case "byte":
                    return new Token(TokenType.Byte);
                case "case":
                    return new Token(TokenType.Case);
                case "catch":
                    return new Token(TokenType.Catch);
                case "char":
                    return new Token(TokenType.Char);
                case "checked":
                    return new Token(TokenType.Checked);
                case "class":
                    return new Token(TokenType.Class);
                case "const":
                    return new Token(TokenType.Const);
                case "continue":
                    return new Token(TokenType.Continue);
                case "decimal":
                    return new Token(TokenType.Decimal);
                case "default":
                    return new Token(TokenType.Default);
                case "delegate":
                    return new Token(TokenType.Delegate);
                case "do":
                    return new Token(TokenType.Do);
                case "double":
                    return new Token(TokenType.Double);
                case "else":
                    return new Token(TokenType.Else);
                case "enum":
                    return new Token(TokenType.Enum);
                case "event":
                    return new Token(TokenType.Event);
                case "explicit":
                    return new Token(TokenType.Explicit);
                case "extern":
                    return new Token(TokenType.Extern);
                case "false":
                    return new Token(TokenType.False);
                case "finally":
                    return new Token(TokenType.Finally);
                case "fixed":
                    return new Token(TokenType.Fixed);
                case "float":
                    return new Token(TokenType.Float);
                case "for":
                    return new Token(TokenType.For);
                case "foreach":
                    return new Token(TokenType.Foreach);
                case "goto":
                    return new Token(TokenType.Goto);
                case "if":
                    return new Token(TokenType.If);
                case "implicit":
                    return new Token(TokenType.Implicit);
                case "in":
                    return new Token(TokenType.In);
                case "int":
                    return new Token(TokenType.Int);
                case "interface":
                    return new Token(TokenType.Interface);
                case "internal":
                    return new Token(TokenType.Internal);
                case "is":
                    return new Token(TokenType.Is);
                case "lock":
                    return new Token(TokenType.Lock);
                case "long":
                    return new Token(TokenType.Long);
                case "namespace":
                    return new Token(TokenType.Namespace);
                case "new":
                    return new Token(TokenType.New);
                case "null":
                    return new Token(TokenType.Null);
                case "object":
                    return new Token(TokenType.Object);
                case "operator":
                    return new Token(TokenType.Operator);
                case "out":
                    return new Token(TokenType.Out);
                case "override":
                    return new Token(TokenType.Override);
                case "params":
                    return new Token(TokenType.Params);
                case "private":
                    return new Token(TokenType.Private);
                case "protected":
                    return new Token(TokenType.Protected);
                case "public":
                    return new Token(TokenType.Public);
                case "readonly":
                    return new Token(TokenType.Readonly);
                case "ref":
                    return new Token(TokenType.Ref);
                case "return":
                    return new Token(TokenType.Return);
                case "sbyte":
                    return new Token(TokenType.Sbyte);
                case "sealed":
                    return new Token(TokenType.Sealed);
                case "short":
                    return new Token(TokenType.Short);
                case "sizeof":
                    return new Token(TokenType.Sizeof);
                case "stackalloc":
                    return new Token(TokenType.Stackalloc);
                case "static":
                    return new Token(TokenType.Static);
                case "string":
                    return new Token(TokenType.String);
                case "struct":
                    return new Token(TokenType.Struct);
                case "switch":
                    return new Token(TokenType.Switch);
                case "this":
                    return new Token(TokenType.This);
                case "throw":
                    return new Token(TokenType.Throw);
                case "true":
                    return new Token(TokenType.True);
                case "try":
                    return new Token(TokenType.Try);
                case "typeof":
                    return new Token(TokenType.Typeof);
                case "uint":
                    return new Token(TokenType.Uint);
                case "ulong":
                    return new Token(TokenType.Ulong);
                case "unchecked":
                    return new Token(TokenType.Unchecked);
                case "unsafe":
                    return new Token(TokenType.Unsafe);
                case "ushort":
                    return new Token(TokenType.Ushort);
                case "using":
                    return new Token(TokenType.Using);
                case "virtual":
                    return new Token(TokenType.Virtual);
                case "void":
                    return new Token(TokenType.Void);
                case "volatile":
                    return new Token(TokenType.Volatile);
                case "while":
                    return new Token(TokenType.While);
                case "add":
                    return new Token(TokenType.Add);
                case "ascending":
                    return new Token(TokenType.Ascending);
                case "async":
                    return new Token(TokenType.Async);
                case "await":
                    return new Token(TokenType.Await);
                case "by":
                    return new Token(TokenType.By);
                case "descending":
                    return new Token(TokenType.Descending);
                case "dynamic":
                    return new Token(TokenType.Dynamic);
                case "equals":
                    return new Token(TokenType.Equals);
                case "file":
                    return new Token(TokenType.File);
                case "from":
                    return new Token(TokenType.From);
                case "get":
                    return new Token(TokenType.Get);
                case "global":
                    return new Token(TokenType.Global);
                case "group":
                    return new Token(TokenType.Group);
                case "init":
                    return new Token(TokenType.Init);
                case "into":
                    return new Token(TokenType.Into);
                case "join":
                    return new Token(TokenType.Join);
                case "let":
                    return new Token(TokenType.Let);
                case "managed":
                    return new Token(TokenType.Managed);
                case "nameof":
                    return new Token(TokenType.Nameof);
                case "nint":
                    return new Token(TokenType.Nint);
                case "not":
                    return new Token(TokenType.Not);
                case "notnull":
                    return new Token(TokenType.Notnull);
                case "nuint":
                    return new Token(TokenType.Nuint);
                case "on":
                    return new Token(TokenType.On);
                case "or":
                    return new Token(TokenType.Or);
                case "orderby":
                    return new Token(TokenType.Orderby);
                case "partial":
                    return new Token(TokenType.Partial);
                case "record":
                    return new Token(TokenType.Record);
                case "remove":
                    return new Token(TokenType.Remove);
                case "required":
                    return new Token(TokenType.Required);
                case "scoped":
                    return new Token(TokenType.Scoped);
                case "select":
                    return new Token(TokenType.Select);
                case "set":
                    return new Token(TokenType.Set);
                case "unmanaged":
                    return new Token(TokenType.Unmanaged);
                case "value":
                    return new Token(TokenType.Value);
                case "var":
                    return new Token(TokenType.Var);
                case "when":
                    return new Token(TokenType.When);
                case "where":
                    return new Token(TokenType.Where);
                case "with":
                    return new Token(TokenType.With);
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
