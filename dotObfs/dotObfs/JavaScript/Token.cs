using System;
using dotObfs.Core.JavaScript;

namespace dotObfs.Core.JavaScript
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
