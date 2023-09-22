using System;
using System.Text;
using System.Text.RegularExpressions;
using dotObfs.Core.VisualBasic;

namespace dotObfs.Core.VisualBasic
{
    public class VisualBasicLexer
    {
        string source;
        int index;
        char current;

        public VisualBasicLexer(string source)
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
                case "AddHandler":
                    return new Token(TokenType.AddHandler);
                case "AddressOf":
                    return new Token(TokenType.AddressOf);
                case "Alias":
                    return new Token(TokenType.Alias);
                case "And":
                    return new Token(TokenType.And);
                case "AndAlso":
                    return new Token(TokenType.AndAlso);
                case "As":
                    return new Token(TokenType.As);
                case "Boolean":
                    return new Token(TokenType.Boolean);
                case "ByRef":
                    return new Token(TokenType.ByRef);
                case "Byte":
                    return new Token(TokenType.Byte);
                case "ByVal":
                    return new Token(TokenType.ByVal);
                case "Call":
                    return new Token(TokenType.Call);
                case "Case":
                    return new Token(TokenType.Case);
                case "Catch":
                    return new Token(TokenType.Catch);
                case "CBool":
                    return new Token(TokenType.CBool);
                case "CByte":
                    return new Token(TokenType.CByte);
                case "CChar":
                    return new Token(TokenType.CChar);
                case "CDate":
                    return new Token(TokenType.CDate);
                case "CDbl":
                    return new Token(TokenType.CDbl);
                case "CDec":
                    return new Token(TokenType.CDec);
                case "Char":
                    return new Token(TokenType.Char);
                case "CInt":
                    return new Token(TokenType.CInt);
                case "Class":
                    return new Token(TokenType.Class);
                case "CLng":
                    return new Token(TokenType.CLng);
                case "CObj":
                    return new Token(TokenType.CObj);
                case "Const":
                    return new Token(TokenType.Const);
                case "Continue":
                    return new Token(TokenType.Continue);
                case "CSByte":
                    return new Token(TokenType.CSByte);
                case "CShort":
                    return new Token(TokenType.CShort);
                case "CSng":
                    return new Token(TokenType.CSng);
                case "CStr":
                    return new Token(TokenType.CStr);
                case "CType":
                    return new Token(TokenType.CType);
                case "CUInt":
                    return new Token(TokenType.CUInt);
                case "CULng":
                    return new Token(TokenType.CULng);
                case "CUShort":
                    return new Token(TokenType.CUShort);
                case "Date":
                    return new Token(TokenType.Date);
                case "Decimal":
                    return new Token(TokenType.Decimal);
                case "Declare":
                    return new Token(TokenType.Declare);
                case "Default":
                    return new Token(TokenType.Default);
                case "Delegate":
                    return new Token(TokenType.Delegate);
                case "Dim":
                    return new Token(TokenType.Dim);
                case "DirectCast":
                    return new Token(TokenType.DirectCast);
                case "Do":
                    return new Token(TokenType.Do);
                case "Double":
                    return new Token(TokenType.Double);
                case "Each":
                    return new Token(TokenType.Each);
                case "Else":
                    return new Token(TokenType.Else);
                case "ElseIf":
                    return new Token(TokenType.ElseIf);
                case "End":
                    return new Token(TokenType.End);
                case "Enum":
                    return new Token(TokenType.Enum);
                case "Erase":
                    return new Token(TokenType.Erase);
                case "Error":
                    return new Token(TokenType.Error);
                case "Event":
                    return new Token(TokenType.Event);
                case "Exit":
                    return new Token(TokenType.Exit);
                case "False":
                    return new Token(TokenType.False);
                case "Finally":
                    return new Token(TokenType.Finally);
                case "For":
                    return new Token(TokenType.For);
                case "Friend":
                    return new Token(TokenType.Friend);
                case "Function":
                    return new Token(TokenType.Function);
                case "Get":
                    return new Token(TokenType.Get);
                case "GetType":
                    return new Token(TokenType.GetType);
                case "GetXMLNamespace":
                    return new Token(TokenType.GetXMLNamespace);
                case "Global":
                    return new Token(TokenType.Global);
                case "GoSub":
                    return new Token(TokenType.GoSub);
                case "GoTo":
                    return new Token(TokenType.GoTo);
                case "Handles":
                    return new Token(TokenType.Handles);
                case "If":
                    return new Token(TokenType.If);
                case "Implements":
                    return new Token(TokenType.Implements);
                case "Imports":
                    return new Token(TokenType.Imports);
                case "In":
                    return new Token(TokenType.In);
                case "Inherits":
                    return new Token(TokenType.Inherits);
                case "Integer":
                    return new Token(TokenType.Integer);
                case "Interface":
                    return new Token(TokenType.Interface);
                case "Is":
                    return new Token(TokenType.Is);
                case "IsNot":
                    return new Token(TokenType.IsNot);
                case "Let":
                    return new Token(TokenType.Let);
                case "Lib":
                    return new Token(TokenType.Lib);
                case "Like":
                    return new Token(TokenType.Like);
                case "Long":
                    return new Token(TokenType.Long);
                case "Loop":
                    return new Token(TokenType.Loop);
                case "Me":
                    return new Token(TokenType.Me);
                case "Mod":
                    return new Token(TokenType.Mod);
                case "Module":
                    return new Token(TokenType.Module);
                case "MustInherit":
                    return new Token(TokenType.MustInherit);
                case "MustOverride":
                    return new Token(TokenType.MustOverride);
                case "MyBase":
                    return new Token(TokenType.MyBase);
                case "MyClass":
                    return new Token(TokenType.MyClass);
                case "NameOf":
                    return new Token(TokenType.NameOf);
                case "Namespace":
                    return new Token(TokenType.Namespace);
                case "Narrowing":
                    return new Token(TokenType.Narrowing);
                case "New":
                    return new Token(TokenType.New);
                case "Next":
                    return new Token(TokenType.Next);
                case "Not":
                    return new Token(TokenType.Not);
                case "Nothing":
                    return new Token(TokenType.Nothing);
                case "NotInheritable":
                    return new Token(TokenType.NotInheritable);
                case "NotOverridable":
                    return new Token(TokenType.NotOverridable);
                case "Object":
                    return new Token(TokenType.Object);
                case "Of":
                    return new Token(TokenType.Of);
                case "On":
                    return new Token(TokenType.On);
                case "Operator":
                    return new Token(TokenType.Operator);
                case "Option":
                    return new Token(TokenType.Option);
                case "Optional":
                    return new Token(TokenType.Optional);
                case "Or":
                    return new Token(TokenType.Or);
                case "OrElse":
                    return new Token(TokenType.OrElse);
                case "Out":
                    return new Token(TokenType.Out);
                case "Overloads":
                    return new Token(TokenType.Overloads);
                case "Overridable":
                    return new Token(TokenType.Overridable);
                case "Overrides":
                    return new Token(TokenType.Overrides);
                case "ParamArray":
                    return new Token(TokenType.ParamArray);
                case "Partial":
                    return new Token(TokenType.Partial);
                case "Private":
                    return new Token(TokenType.Private);
                case "Property":
                    return new Token(TokenType.Property);
                case "Protected":
                    return new Token(TokenType.Protected);
                case "Public":
                    return new Token(TokenType.Public);
                case "RaiseEvent":
                    return new Token(TokenType.RaiseEvent);
                case "ReadOnly":
                    return new Token(TokenType.ReadOnly);
                case "ReDim":
                    return new Token(TokenType.ReDim);
                case "REM":
                    return new Token(TokenType.REM);
                case "RemoveHandler":
                    return new Token(TokenType.RemoveHandler);
                case "Resume":
                    return new Token(TokenType.Resume);
                case "Return":
                    return new Token(TokenType.Return);
                case "SByte":
                    return new Token(TokenType.SByte);
                case "Select":
                    return new Token(TokenType.Select);
                case "Set":
                    return new Token(TokenType.Set);
                case "Shadows":
                    return new Token(TokenType.Shadows);
                case "Shared":
                    return new Token(TokenType.Shared);
                case "Short":
                    return new Token(TokenType.Short);
                case "Single":
                    return new Token(TokenType.Single);
                case "Static":
                    return new Token(TokenType.Static);
                case "Step":
                    return new Token(TokenType.Step);
                case "Stop":
                    return new Token(TokenType.Stop);
                case "String":
                    return new Token(TokenType.String);
                case "Structure":
                    return new Token(TokenType.Structure);
                case "Sub":
                    return new Token(TokenType.Sub);
                case "SyncLock":
                    return new Token(TokenType.SyncLock);
                case "Then":
                    return new Token(TokenType.Then);
                case "Throw":
                    return new Token(TokenType.Throw);
                case "To":
                    return new Token(TokenType.To);
                case "True":
                    return new Token(TokenType.True);
                case "Try":
                    return new Token(TokenType.Try);
                case "TryCast":
                    return new Token(TokenType.TryCast);
                case "TypeOf":
                    return new Token(TokenType.TypeOf);
                case "UInteger":
                    return new Token(TokenType.UInteger);
                case "ULong":
                    return new Token(TokenType.ULong);
                case "UShort":
                    return new Token(TokenType.UShort);
                case "Using":
                    return new Token(TokenType.Using);
                case "Variant":
                    return new Token(TokenType.Variant);
                case "Wend":
                    return new Token(TokenType.Wend);
                case "When":
                    return new Token(TokenType.When);
                case "While":
                    return new Token(TokenType.While);
                case "Widening":
                    return new Token(TokenType.Widening);
                case "With":
                    return new Token(TokenType.With);
                case "WithEvents":
                    return new Token(TokenType.WithEvents);
                case "WriteOnly":
                    return new Token(TokenType.WriteOnly);
                case "Xor":
                    return new Token(TokenType.Xor);
                case "Aggregate":
                    return new Token(TokenType.Aggregate);
                case "Auto":
                    return new Token(TokenType.Auto);
                case "Custom":
                    return new Token(TokenType.Custom);
                case "From":
                    return new Token(TokenType.From);
                case "IsFalse":
                    return new Token(TokenType.IsFalse);
                case "Key":
                    return new Token(TokenType.Key);
                case "Preserve":
                    return new Token(TokenType.Preserve);
                case "Take":
                    return new Token(TokenType.Take);
                case "Until":
                    return new Token(TokenType.Until);
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
