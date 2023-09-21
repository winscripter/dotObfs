using System;
using dotObfs.Core.CSharp;

namespace dotObfs.Core.CSharp
{
    public class Token
    {
        public TokenType type;
        public string value;

        public Token(TokenType type, string value)
        {
            this.type = type;
            this.value = value;
        }

        public Token(TokenType type)
        {
            this.type = type;
        }

        public Token()
        {
        }
    }
}
