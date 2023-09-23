using System;

namespace dotObfs.Core.C
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
