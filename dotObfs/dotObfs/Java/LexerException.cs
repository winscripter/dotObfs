using System;

namespace dotObfs.Core.Java
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
