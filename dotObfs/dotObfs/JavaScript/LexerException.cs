using System;

namespace dotObfs.Core.JavaScript
{
    internal class LexerException : Exception
    {
        public LexerException(string msg)
            : base (msg)
        {
        }

        public LexerException()
        {
        }
    }
}
