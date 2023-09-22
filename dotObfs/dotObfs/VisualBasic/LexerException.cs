using System;

namespace dotObfs.Core.VisualBasic
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
